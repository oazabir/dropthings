#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace AJAXASMXHandler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Transactions;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Script.Serialization;
    using System.Web.Services;

    public class ASMXHttpHandler : IHttpAsyncHandler
    {
        #region Fields

        private readonly static string[] AllowedTypes = new string[] { "application/json", "text/xml" };

        private static readonly Type DictionaryPairType = typeof(Dictionary<string, object>);

        #endregion Fields

        #region Properties

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        #endregion Properties

        #region Methods

        IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            // Proper Content-Type header must be present in order to make a REST call
            if (!IsRestMethodCall(context.Request))
            {
                return GenerateErrorResponse(context, "Not a valid REST call", extraData);
            }

            string methodName = context.Request.PathInfo.Substring(1);

            WebServiceDef wsDef = WebServiceHelper.GetWebServiceType(context, context.Request.FilePath);
            WebMethodDef methodDef = wsDef.Methods[methodName];

            if (null == methodDef) return GenerateErrorResponse(context, "Web method not supported: " + methodName, extraData);

            // GET request will only be allowed if the method says so
            if (context.Request.HttpMethod == "GET" && !methodDef.IsGetAllowed)
                return GenerateErrorResponse(context, "Http Get method not supported", extraData);

            context.Response.Filter = new ResponseFilter(context.Response);

            // If the method does not have a BeginXXX and EndXXX pair, execute it synchronously
            if (!methodDef.HasAsyncMethods)
            {
                // Do synchronous call
                ExecuteMethod(context, methodDef, wsDef);

                // Return a result that says method was executed synchronously
                return new AsmxHandlerSyncResult(extraData);
            }
            else
            {
                // Call the Begin method of web service
                IDisposable target = Activator.CreateInstance(wsDef.WSType) as IDisposable;

                WebMethodDef beginMethod = methodDef.BeginMethod;
                int allParameterCount = beginMethod.InputParametersWithAsyc.Count;

                IDictionary<string, object> inputValues = GetRawParams(context, beginMethod.InputParameters, wsDef.Serializer);
                object[] parameterValues = StrongTypeParameters(inputValues, beginMethod.InputParameters);

                // Prepare the list of parameter values which will also include the AsyncCallback and the state
                object[] parameterValuesWithAsync = new object[allParameterCount];
                Array.Copy(parameterValues, parameterValuesWithAsync, parameterValues.Length);

                // Populate last two parameters with async callback and state
                parameterValuesWithAsync[allParameterCount - 2] = cb;

                AsyncWebMethodState webMethodState = new AsyncWebMethodState(methodName, target,
                    wsDef, methodDef, context, extraData);
                parameterValuesWithAsync[allParameterCount - 1] = webMethodState;

                try
                {
                    // Invoke the BeginXXX method and ensure the return result has AsyncWebMethodState. This state
                    // contains context and other information which we need in oreder to call the EndXXX
                    IAsyncResult result = beginMethod.MethodType.Invoke(target, parameterValuesWithAsync) as IAsyncResult;

                    // If execution has completed synchronously within the BeginXXX function, then generate response
                    // immediately. There's no need to call EndXXX
                    if (result.CompletedSynchronously)
                    {
                        object returnValue = result.AsyncState;
                        GenerateResponse(returnValue, context, methodDef, wsDef);

                        target.Dispose();
                        return new AsmxHandlerSyncResult(extraData);
                    }
                    else
                    {
                        if (result.AsyncState is AsyncWebMethodState) return result;
                        else throw new InvalidAsynchronousStateException("The state passed in the " + beginMethod.MethodName + " must inherit from " + typeof(AsyncWebMethodState).FullName);
                    }
                }
                catch (Exception x)
                {
                    target.Dispose();
                    WebServiceHelper.WriteExceptionJsonString(context, x, wsDef.Serializer);
                    return new AsmxHandlerSyncResult(extraData);
                }
            }
        }

        void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result)
        {
            if (result.CompletedSynchronously) return;

            AsyncWebMethodState state = result.AsyncState as AsyncWebMethodState;

            if (result.IsCompleted)
            {
                MethodInfo endMethod = state.MethodDef.EndMethod.MethodType;

                try
                {
                    object returnValue = endMethod.Invoke(state.Target, new object[] { result });
                    GenerateResponse(returnValue, state.Context, state.MethodDef, state.ServiceDef);
                }
                catch (Exception x)
                {
                    WebServiceHelper.WriteExceptionJsonString(state.Context, x, state.ServiceDef.Serializer);
                }
                finally
                {
                    state.Target.Dispose();
                }

                state.Dispose();
            }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            // Code is in BeginProcessRequest
        }

        internal static bool IsInheritFrom(Type child, Type inheritFrom)
        {
            Type parent = child.BaseType;
            while (parent != null)
            {
                if (parent == inheritFrom) return true;
                else parent = parent.BaseType;
            }

            return false;
        }

        internal static bool IsRestMethodCall(HttpRequest request)
        {
            return !String.IsNullOrEmpty(request.PathInfo) &&
                Array.Exists(AllowedTypes, type => request.ContentType.Contains(type));
        }

        // Hash an input string and return the hash as
        // a 32 character hexadecimal string.
        static string GetMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private static IDictionary<string, object> GetRawParams(HttpContext context, List<ParameterInfo> parameters, JavaScriptSerializer serializer)
        {
            // TODO: Security check for UseGet attribute's presence
            if (context.Request.HttpMethod == "GET")
            {
                return GetRawParamsFromGetRequest(context, serializer, parameters);
            }
            else if (context.Request.HttpMethod == "POST")
            {
                return GetRawParamsFromPostRequest(context, serializer);
            }
            else
            {
                throw new InvalidOperationException("Unknown HTTP Method");
            }
        }

        private static IDictionary<string, object> GetRawParamsFromGetRequest(HttpContext context, JavaScriptSerializer serializer, List<ParameterInfo> parameters)
        {
            NameValueCollection queryString = context.Request.QueryString;
            Dictionary<string, object> rawParams = new Dictionary<string, object>();

            StringBuilder buffer = new StringBuilder();
            buffer.Append('{');
            foreach (ParameterInfo param in parameters)
            {
                string name = param.Name;
                string val = queryString[name];
                if (val != null)
                {
                    buffer.Append(name);
                    buffer.Append(':');
                    buffer.Append(val);
                    buffer.Append(',');
                }
            }

            if (buffer.Length > 1) buffer.Length--;
            buffer.Append('}');
            return serializer.Deserialize<IDictionary<string, object>>(buffer.ToString());
        }

        private static IDictionary<string, object> GetRawParamsFromPostRequest(HttpContext context, JavaScriptSerializer serializer)
        {
            TextReader reader = new StreamReader(context.Request.InputStream);
            string bodyString = reader.ReadToEnd();

            if (String.IsNullOrEmpty(bodyString))
            {
                return new Dictionary<string, object>();
            }

            return serializer.Deserialize<IDictionary<string, object>>(bodyString);
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(input);

            // Create a StringComparer an comare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Execute web method synchronously
        /// </summary>
        /// <param name="context"></param>
        /// <param name="methodDef"></param>
        /// <param name="serviceDef"></param>
        private void ExecuteMethod(HttpContext context, WebMethodDef methodDef, WebServiceDef serviceDef)
        {
            IDictionary<string, object> inputValues = GetRawParams(context, methodDef.InputParameters, serviceDef.Serializer);
            object[] parameters = StrongTypeParameters(inputValues, methodDef.InputParameters);

            object returnValue = null;
            using (IDisposable target = Activator.CreateInstance(serviceDef.WSType) as IDisposable)
            {
                TransactionScope ts = null;
                try
                {
                    // If the method has a transaction attribute, then call the method within a transaction scope
                    if (methodDef.TransactionAtt != null)
                    {
                        TransactionOptions options = new TransactionOptions();
                        options.IsolationLevel = methodDef.TransactionAtt.IsolationLevel;
                        options.Timeout = TimeSpan.FromSeconds( methodDef.TransactionAtt.Timeout );

                        ts = new TransactionScope(methodDef.TransactionAtt.TransactionOption, options);
                    }

                    returnValue = methodDef.MethodType.Invoke(target, parameters);

                    // If transaction was used, then complete the transaction because no exception was
                    // generated
                    if( null != ts ) ts.Complete();

                    GenerateResponse(returnValue, context, methodDef, serviceDef);
                }
                catch (Exception x)
                {
                    WebServiceHelper.WriteExceptionJsonString(context, x, serviceDef.Serializer);
                }
                finally
                {
                    // If transaction was started for the method, dispose the transaction. This will
                    // rollback if not committed
                    if( null != ts) ts.Dispose();

                    // Dispose the web service
                    target.Dispose();
                }
            }
        }

        private IAsyncResult GenerateErrorResponse(HttpContext context, string message, object extraData)
        {
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            context.Response.StatusDescription = HttpWorkerRequest.GetStatusDescription((int)HttpStatusCode.MethodNotAllowed);
            context.Response.Write(message);

            return new AsmxHandlerSyncResult(extraData);
        }

        private void GenerateResponse(object returnValue, HttpContext context, WebMethodDef methodDef, WebServiceDef serviceDef)
        {
            if (context.Response.Filter.Length > 0)
            {
                // Response has already been transmitted by the WebMethod.
                // So, do nothing
                return;
            }
            string responseString = null;
            string contentType = "application/json";

            if (methodDef.ResponseFormat == System.Web.Script.Services.ResponseFormat.Json)
            {
                responseString = "{ d : (" + serviceDef.Serializer.Serialize(returnValue) + ")}";
                contentType = "application/json";
            }
            else if (methodDef.ResponseFormat == System.Web.Script.Services.ResponseFormat.Xml)
            {
                responseString = returnValue as string;
                contentType = "text/xml";
            }

            context.Response.ContentType = contentType;

            // If we have response and no redirection happening and client still connected, send response
            if (responseString != null
                && !context.Response.IsRequestBeingRedirected
                && context.Response.IsClientConnected)
            {
                // Produce proper cache. If no cache information specified on method and there's been no cache related
                // changes done within the web method code, then default cache will be private, no cache.
                if (IsCacheSet(context.Response) || methodDef.IsETagEnabled)
                {
                    // Cache has been modified within the code. So, do not change any cache policy
                }
                else
                {
                    // Cache is still private. Check if there's any CacheDuration set in WebMethod
                    int cacheDuration = methodDef.WebMethodAtt.CacheDuration;
                    if (cacheDuration > 0)
                    {
                        // If CacheDuration attribute is set, use server side caching
                        context.Response.Cache.SetCacheability(HttpCacheability.Server);
                        context.Response.Cache.SetExpires(DateTime.Now.AddSeconds(cacheDuration));
                        context.Response.Cache.SetSlidingExpiration(false);
                        context.Response.Cache.SetValidUntilExpires(true);

                        if (methodDef.InputParameters.Count > 0)
                        {
                            context.Response.Cache.VaryByParams["*"] = true;
                        }
                        else
                        {
                            context.Response.Cache.VaryByParams.IgnoreParams = true;
                        }
                    }
                    else
                    {
                        context.Response.Cache.SetNoServerCaching();
                        context.Response.Cache.SetMaxAge(TimeSpan.Zero);
                    }
                }

                // Check if there's any need to do ETag match. If ETag matches, produce HTTP 304, otherwise
                // render the content along with the ETag
                if (methodDef.IsETagEnabled)
                {
                    string etag = context.Request.Headers["If-None-Match"];
                    string hash = GetMd5Hash(responseString);

                    if (!string.IsNullOrEmpty(etag))
                    {
                        if (string.Compare(hash, etag, true) == 0)
                        {
                            // Send no body as response and we will just abort it
                            context.Response.ClearContent();
                            context.Response.AppendHeader("Content-Length", "0");
                            context.Response.SuppressContent = true;
                            context.Response.StatusCode = 304;

                            // No need to produce output response body
                            return;
                        }
                    }

                    // ETag comparison did not happen or comparison did not match. So, we need to produce new ETag
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
                    HttpContext.Current.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
                    HttpContext.Current.Response.Cache.SetETag(hash);
                    HttpContext.Current.Response.Cache.SetLastModified(DateTime.Now);

                    int cacheDuration = methodDef.WebMethodAtt.CacheDuration;
                    if (cacheDuration > 0)
                    {
                        context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(cacheDuration));
                        context.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(cacheDuration));
                    }
                    else
                    {
                        context.Response.Cache.SetMaxAge(TimeSpan.FromSeconds(10));
                    }

                }

                // Convert the response to response encoding, e.g. utf8
                byte[] unicodeBytes = Encoding.Unicode.GetBytes(responseString);
                byte[] utf8Bytes = Encoding.Convert(Encoding.Unicode, context.Response.ContentEncoding, unicodeBytes);

                // Emit content length in UTF8 encoding string
                context.Response.AppendHeader("Content-Length", utf8Bytes.Length.ToString());

                // Instead of Response.Write which will convert the output to UTF8, use the internal stream
                // to directly write the utf8 bytes
                context.Response.OutputStream.Write(utf8Bytes, 0, utf8Bytes.Length);
            }
            else
            {
                // Send no body as response and we will just abort it
                context.Response.AppendHeader("Content-Length", "0");
                context.Response.ClearContent();
                context.Response.StatusCode = 204; // No Content
            }
        }

        private bool IsCacheSet(HttpResponse response)
        {
            if (response.CacheControl == "public") return true;

            FieldInfo maxAgeField = response.Cache.GetType().GetField("_maxAge", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
            TimeSpan maxAgeValue = (TimeSpan)maxAgeField.GetValue(response.Cache);

            if (maxAgeValue != TimeSpan.Zero) return true;

            return false;
        }

        /// <summary>
        /// Converts the parameter values to proper data type according to the parameter data types
        /// For example, a string value in input parameter is converted to integer if prameter type is integer
        /// </summary>
        /// <param name="rawParams"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object[] StrongTypeParameters(IDictionary<string, object> rawParams, List<ParameterInfo> parameters)
        {
            object[] returnValues = new object[parameters.Count];
            int i = 0;

            foreach(ParameterInfo param in parameters)
            {
                string paramName = param.Name;

                if (rawParams.ContainsKey(paramName))
                {
                    Type paramType = param.ParameterType;
                    object val = rawParams[paramName];

                    if (null != val)
                    {
                        // Convert dictionary from JSON to SerializableDictionary if the parameter is of SerializableDictionary
                        if (val is Dictionary<string,object>)
                        {
                            if (paramType == DictionaryPairType)
                            {
                                // leave unchanged, dictionary will be mapped to dictionary property directly
                            }
                            else if (IsInheritFrom(paramType, DictionaryPairType))
                            {
                                // Inherits from Dictionary<string,object>. Construct the object by passing the
                                // dictionary as argument
                                val = Activator.CreateInstance(paramType, val);
                            }
                            else if (paramType.IsAssignableFrom( DictionaryPairType ))
                            {
                                // leave unchanged, as it can be assigned
                            }
                            else
                            {
                                // object needs to be mapped from whatever is in dictionary. Dictionary was sent
                                // without __type parameter. So, javascript deserializer does not know what
                                // object to contruct
                                val = null;
                            }
                        }
                        else if (val is object[])
                        {
                            if (paramType.IsArray)
                            {
                                object[] sourceArray = (object[])val;

                                if (paramType == typeof(int[]))
                                {
                                    int[] newArray = new int[sourceArray.Length];
                                Array.Copy(sourceArray, newArray, sourceArray.Length);
                                val = newArray;
                            }
                                else
                                {

                                    object[] newArray = Activator.CreateInstance(paramType, (sourceArray.Length)) as object[];
                                    Array.Copy(sourceArray, newArray, sourceArray.Length);
                                    val = newArray;
                                }

                            }
                        }
                        else
                        {
                            Type valueType = val.GetType();

                            TypeConverter converter = TypeDescriptor.GetConverter(paramType);

                            if (valueType != paramType && !paramType.IsAssignableFrom(valueType))
                            {
                                if (converter.CanConvertFrom(valueType))
                                {
                                    val = converter.ConvertFrom(null, CultureInfo.InvariantCulture, val);
                                }

                                if (converter.CanConvertFrom(typeof(String)))
                                {
                                    TypeConverter propertyConverter = TypeDescriptor.GetConverter(val);

                                    string s = propertyConverter.ConvertToInvariantString(val);
                                    val = converter.ConvertFromInvariantString(s);
                                }
                            }
                        }
                    }

                    returnValues[i++] = val;
                }
                else
                {
                    // Parameter was not supplied in request, pass null as parameter value
                    returnValues[i++] = null;
                }
            }
            return returnValues;
        }

        #endregion Methods
    }

    public class AsmxHandlerSyncResult : IAsyncResult
    {
        #region Fields

        private WaitHandle handle = new ManualResetEvent(true);
        private object state;

        #endregion Fields

        #region Constructors

        public AsmxHandlerSyncResult(object state)
        {
            this.state = state;
        }

        #endregion Constructors

        #region Properties

        object IAsyncResult.AsyncState
        {
            get { return this.state; }
        }

        WaitHandle IAsyncResult.AsyncWaitHandle
        {
            get { return this.handle; }
        }

        bool IAsyncResult.CompletedSynchronously
        {
            get { return true; }
        }

        bool IAsyncResult.IsCompleted
        {
            get { return true; }
        }

        #endregion Properties
    }

    internal class TypeResolver : JavaScriptTypeResolver
    {
        #region Methods

        public override Type ResolveType(string id)
        {
            return Type.GetType(id);
        }

        public override string ResolveTypeId(Type type)
        {
            return type.FullName + "," + type.Assembly.GetName().Name;
        }

        #endregion Methods
    }
}