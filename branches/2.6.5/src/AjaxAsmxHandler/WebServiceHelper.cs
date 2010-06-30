#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace AJAXASMXHandler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Security;
    using System.Text;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Compilation;
    using System.Web.Configuration;
    using System.Web.Hosting;
    using System.Web.Script.Serialization;
    using System.Web.Script.Services;
    using System.Web.Services;

    internal class JsonTypeResolver : JavaScriptTypeResolver
    {
        #region Fields

        private static readonly Type[] s_emptyTypeArray = new Type[] { };

        private Dictionary<string, Type> _clientTypesDictionary;
        private bool _clientTypesProcessed;
        private Dictionary<Type, object> _enumTypesDictionary;
        private Hashtable _processedTypes;
        private JavaScriptSerializer _serializer;
        private WebServiceDef _typeData;
        private Dictionary<string, string> _typeResolverSpecials = new Dictionary<string, string>();

        #endregion Fields

        #region Constructors

        public JsonTypeResolver(WebServiceDef def)
        {
            this._serializer = new JavaScriptSerializer(this);
            this._typeData = def;
        }

        #endregion Constructors

        #region Properties

        internal Dictionary<string, Type> ClientTypeDictionary
        {
            get
            {
                EnsureClientTypesProcessed();
                return _clientTypesDictionary;
            }
            set
            {
                _clientTypesDictionary = value;
            }
        }

        internal IEnumerable<Type> ClientTypes
        {
            get
            {
                return ClientTypeDictionary.Values;
            }
        }

        internal Dictionary<Type, object> EnumTypeDictionary
        {
            get
            {
                EnsureClientTypesProcessed();
                return _enumTypesDictionary;
            }
            set
            {
                _enumTypesDictionary = value;
            }
        }

        internal IEnumerable<Type> EnumTypes
        {
            get
            {
                EnsureClientTypesProcessed();
                return _enumTypesDictionary.Keys;
            }
        }

        internal JavaScriptSerializer Serializer
        {
            get
            {
                return _serializer;
            }
        }

        #endregion Properties

        #region Methods

        public override Type ResolveType(string id)
        {
            Type type = null;
            if (ClientTypeDictionary.TryGetValue(id, out type))
            {
                return type;
            }
            else
            {
                Type resolvedType = Type.GetType(id);
                if (null == resolvedType)
                    return null;
                else
                {
                    ClientTypeDictionary.Add(id, resolvedType);
                    return resolvedType;
                }
            }
        }

        public override string ResolveTypeId(Type type)
        {
            string typeString = GetTypeStringRepresentation(type.FullName);

            if (!ClientTypeDictionary.ContainsKey(typeString))
                return null;

            return typeString;
        }

        internal static bool IsClientInstantiatableType(Type t, JavaScriptSerializer serializer)
        {
            if (t.IsAbstract || t.IsInterface || t.IsArray)
                return false;

            if (t == typeof(object))
                return false;

            /*
            JavaScriptConverter converter = null;
            if (serializer.ConverterExistsForType(t, out converter))
            {
                return true;
            }
            */

            if (t.IsValueType)
            {
                return true;
            }

            ConstructorInfo constructorInfo = t.GetConstructor(BindingFlags.Public | BindingFlags.Instance,
                null, s_emptyTypeArray, null);
            if (constructorInfo == null)
                return false;

            return true;
        }

        internal string GetTypeStringRepresentation(string typeName)
        {
            return GetTypeStringRepresentation(typeName, true);
        }

        internal string GetTypeStringRepresentation(string typeName, bool ensure)
        {
            if (ensure)
            {
                EnsureClientTypesProcessed();
            }

            string typeString;
            if (_typeResolverSpecials.TryGetValue(typeName, out typeString))
            {
                return typeString;
            }

            return typeName;
        }

        private void EnsureClientTypesProcessed()
        {
            if (_clientTypesProcessed)
                return;

            lock (this)
            {
                if (_clientTypesProcessed)
                    return;
                ProcessClientTypes();
            }
        }

        private void ProcessClientType(Type t)
        {
            ProcessClientType(t, false);
        }

        private void ProcessClientType(Type t, bool force)
        {
            if (!force && _processedTypes.Contains(t))
                return;
            _processedTypes[t] = null;

            if (t.IsEnum)
            {
                _enumTypesDictionary[t] = null;
                return;
            }

            if (t.IsGenericType)
            {
                Type[] genericArgs = t.GetGenericArguments();
                if (genericArgs.Length > 1)
                {
                    return;
                }
                ProcessClientType(genericArgs[0]);
            }

            else if (t.IsArray)
            {
                ProcessClientType(t.GetElementType());
            }
            else
            {
                if (t.IsPrimitive || t == typeof(object) || t == typeof(string) || t == typeof(DateTime) ||
                    t == typeof(void) || t == typeof(System.Decimal) || t == typeof(Guid) ||
                    typeof(IEnumerable).IsAssignableFrom(t) || typeof(IDictionary).IsAssignableFrom(t) ||
                    !IsClientInstantiatableType(t, _serializer))
                    return;

                _clientTypesDictionary[GetTypeStringRepresentation(t.FullName, false)] = t;
            }
        }

        private void ProcessClientTypes()
        {
            _clientTypesDictionary = new Dictionary<string, Type>();
            _enumTypesDictionary = new Dictionary<Type, object>();

            try
            {
                _processedTypes = new Hashtable();
                ProcessIncludeAttributes((GenerateScriptTypeAttribute[])_typeData.WSType.GetCustomAttributes(typeof(GenerateScriptTypeAttribute), true));

                foreach (WebMethodDef methodData in this._typeData.Methods.Values)
                {
                    ProcessIncludeAttributes((GenerateScriptTypeAttribute[])methodData.MethodType.GetCustomAttributes(typeof(GenerateScriptTypeAttribute), true));
                    foreach (ParameterInfo paramData in methodData.InputParameters)
                    {
                        ProcessClientType(paramData.ParameterType);
                    }
                    if (methodData.ResponseFormat == ResponseFormat.Xml) continue;
                    ProcessClientType(methodData.MethodType.ReturnType);
                }
            }
            catch
            {

                _clientTypesDictionary = null;
                _enumTypesDictionary = null;
                throw;
            }
            finally
            {
                _processedTypes = null;
                _clientTypesProcessed = true;
            }
        }

        private void ProcessIncludeAttributes(GenerateScriptTypeAttribute[] attributes)
        {
            foreach (GenerateScriptTypeAttribute attribute in attributes)
            {
                if (!string.IsNullOrEmpty(attribute.ScriptTypeId))
                    _typeResolverSpecials[attribute.Type.FullName] = attribute.ScriptTypeId;

                Type t = attribute.Type;
                if (t.IsPrimitive || t == typeof(object) || t == typeof(string) ||
                    t == typeof(DateTime) || t == typeof(Guid) ||
                    typeof(IEnumerable).IsAssignableFrom(t) || typeof(IDictionary).IsAssignableFrom(t) ||
                    (t.IsGenericType && t.GetGenericArguments().Length > 1) ||
                    !IsClientInstantiatableType(t, _serializer))
                    throw new InvalidOperationException("Invalid type: " + t.Name);

                ProcessClientType(t, true);
            }
        }

        #endregion Methods
    }

    /// <summary>
    /// Caches a web method definition along with its BeginXXX and EndXXX pairs
    /// </summary>
    internal class WebMethodDef
    {
        #region Fields

        public const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly;

        public WebMethodDef BeginMethod;
        public WebMethodDef EndMethod;
        public bool HasAsyncMethods;
        public List<ParameterInfo> InputParameters;
        public List<ParameterInfo> InputParametersWithAsyc;
        public bool IsETagEnabled;
        public bool IsGetAllowed;
        public string MethodName;
        public MethodInfo MethodType;
        public ResponseFormat ResponseFormat;
        public ScriptMethodAttribute ScriptMethodAtt;
        public TransactionalMethodAttribute TransactionAtt;
        public WebMethodAttribute WebMethodAtt;

        #endregion Fields

        #region Constructors

        public WebMethodDef(WebServiceDef wsDef, MethodInfo method, WebMethodAttribute wmAttribute, ScriptMethodAttribute smAttribute, TransactionalMethodAttribute tmAttribute, ETagMethodAttribute emAttribute)
        {
            this.MethodType = method;
            this.WebMethodAtt = wmAttribute;
            this.ScriptMethodAtt = smAttribute;
            this.TransactionAtt = tmAttribute;

            if( null != emAttribute ) this.IsETagEnabled = emAttribute.Enabled;

            if (wmAttribute != null && !string.IsNullOrEmpty(wmAttribute.MessageName)) this.MethodName = wmAttribute.MessageName;
            else this.MethodName = method.Name;

            // HTTP GET method is allowed only when there's a [ScriptMethod] attribute and UseHttpGet is true
            this.IsGetAllowed = (this.ScriptMethodAtt != null && this.ScriptMethodAtt.UseHttpGet);

            this.ResponseFormat = (this.ScriptMethodAtt != null ? this.ScriptMethodAtt.ResponseFormat : ResponseFormat.Json);

            MethodInfo beginMethod = wsDef.WSType.GetMethod("Begin" + method.Name, BINDING_FLAGS);
            if (null != beginMethod)
            {
                // The BeginXXX method must have the [ScriptMethod] attribute
                object[] scriptMethodAttributes = beginMethod.GetCustomAttributes(typeof(ScriptMethodAttribute), false);
                if (scriptMethodAttributes.Length > 0)
                {
                    // Asynchronous methods found for the function
                    this.HasAsyncMethods = true;

                    this.BeginMethod = new WebMethodDef(wsDef, beginMethod, null, null, null, null);

                    MethodInfo endMethod = wsDef.WSType.GetMethod("End" + method.Name, BINDING_FLAGS);
                    this.EndMethod = new WebMethodDef(wsDef, endMethod, null, null, null, null);

                    // get all parameters of begin web method and then leave last two parameters in the input parameters list because
                    // last two parameters are for AsyncCallback and Async State
                    ParameterInfo[] allParameters = beginMethod.GetParameters();
                    ParameterInfo[] inputParameters = new ParameterInfo[allParameters.Length - 2];
                    Array.Copy(allParameters, inputParameters, allParameters.Length - 2);

                    this.BeginMethod.InputParameters = new List<ParameterInfo>(inputParameters);
                    this.BeginMethod.InputParametersWithAsyc = new List<ParameterInfo>(allParameters);
                }
            }

            this.InputParameters = new List<ParameterInfo>(method.GetParameters());
        }

        #endregion Constructors
    }

    internal class WebServiceDef
    {
        #region Fields

        public Dictionary<string, WebMethodDef> Methods;
        public Type WSType;

        private JsonTypeResolver _TypeResolver;

        #endregion Fields

        #region Constructors

        public WebServiceDef(Type wsType)
        {
            this.WSType = wsType;

            this.EnsureMethods(wsType);

            this._TypeResolver = new JsonTypeResolver(this);
        }

        #endregion Constructors

        #region Properties

        public JavaScriptSerializer Serializer
        {
            get { return this._TypeResolver.Serializer; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Caches a web method definition 
        /// </summary>
        /// <param name="wsType"></param>
        /// <param name="methods"></param>
        /// <param name="method"></param>
        private void AddMethod(Type wsType, Dictionary<string, WebMethodDef> methods, MethodInfo method)
        {
            object[] wmAttribs = method.GetCustomAttributes(typeof(WebMethodAttribute), false);

            if (wmAttribs.Length == 0)
                return;

            ScriptMethodAttribute sm = null;
            object[] responseAttribs = method.GetCustomAttributes(typeof(ScriptMethodAttribute), false);
            if (responseAttribs.Length > 0)
            {
                sm = (ScriptMethodAttribute)responseAttribs[0];
            }

            TransactionalMethodAttribute tm = null;
            object[] tmAttribs = method.GetCustomAttributes(typeof(TransactionalMethodAttribute), false);
            if (tmAttribs.Length > 0)
            {
                tm = (TransactionalMethodAttribute)tmAttribs[0];
            }

            ETagMethodAttribute em = null;
            object[] emAttribs = method.GetCustomAttributes(typeof(ETagMethodAttribute), false);
            if (emAttribs.Length > 0)
            {
                em = (ETagMethodAttribute)emAttribs[0];
            }

            WebMethodDef wmd = new WebMethodDef(this, method, (WebMethodAttribute)wmAttribs[0], sm, tm, em);
            methods[wmd.MethodName] = wmd;
        }

        /// <summary>
        /// Scans the web service type for Web methods and precaches web method definition
        /// </summary>
        /// <param name="wsType"></param>
        private void EnsureMethods(Type wsType)
        {
            lock (this)
            {
                List<Type> typeList = new List<Type>();

                // Run through the inheritence tree and get all base types
                Type current = wsType;
                typeList.Add(current);
                while (current.BaseType != null)
                {
                    current = current.BaseType;
                    typeList.Add(current);
                }

                // Find all Web methods in the web service and its base class inheritence tree
                Dictionary<string, WebMethodDef> methods = new Dictionary<string, WebMethodDef>(StringComparer.OrdinalIgnoreCase);

                for (int i = typeList.Count - 1; i >= 0; --i)
                {
                    MethodInfo[] methodInfos = typeList[i].GetMethods(WebMethodDef.BINDING_FLAGS);
                    foreach (MethodInfo method in methodInfos)
                    {
                        AddMethod(wsType, methods, method);
                    }
                }

                this.Methods = methods;
            }
        }

        #endregion Methods
    }

    internal class WebServiceHelper
    {
        #region Methods

        internal static string GetCacheKey(string vpath)
        {
            return "ASMXHandler.WebServiceHelper:" + vpath;
        }

        internal static WebServiceDef GetWebServiceType(HttpContext context, string virtualPath)
        {
            virtualPath = VirtualPathUtility.ToAbsolute(virtualPath.Replace(".soap", ".asmx"));

            string cacheKey = GetCacheKey(virtualPath);
            WebServiceDef wsType = context.Cache[cacheKey] as WebServiceDef;

            if (wsType == null)
            {
                if (HostingEnvironment.VirtualPathProvider.FileExists(virtualPath))
                {
                    Type compiledType = null;
                    try
                    {
                        compiledType = BuildManager.GetCompiledType(virtualPath);

                        if (compiledType == null)
                        {
                            object page = BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(System.Web.UI.Page));
                            compiledType = page.GetType();
                        }

                    }
                    catch (SecurityException)
                    {
                    }

                    if (compiledType != null)
                    {
                        wsType = new WebServiceDef(compiledType);
                        BuildDependencySet deps = BuildManager.GetCachedBuildDependencySet(context, virtualPath);
                        IEnumerable virtualPaths = deps.VirtualPaths;
                        if (virtualPaths != null)
                        {
                            List<string> paths = new List<string>();
                            foreach (string path in virtualPaths)
                            {
                                paths.Add(Path.Combine(context.Request.PhysicalApplicationPath, VirtualPathUtility.GetFileName(path)));
                            }
                            context.Cache.Insert(cacheKey, wsType, new CacheDependency(paths.ToArray()));
                        }
                        else
                        {
                            context.Cache.Insert(cacheKey, wsType);
                        }
                    }
                }
            }

            if (wsType == null)
            {
                throw new InvalidOperationException("Webservice does not exist");
            }

            return wsType;
        }

        internal static void WriteExceptionJsonString(HttpContext context, Exception ex, JavaScriptSerializer serializer)
        {
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.StatusDescription = HttpWorkerRequest.GetStatusDescription((int)HttpStatusCode.InternalServerError);
            context.Response.ContentType = "application/json";
            context.Response.AddHeader("jsonerror", "true");
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, new UTF8Encoding(false)))
            {
                if (ex is TargetInvocationException)
                {
                    ex = ex.InnerException;
                }

                context.AddError(ex);

                if (context.IsCustomErrorEnabled)
                {
                    writer.Write(serializer.Serialize(new WebServiceError("Error occured while processing request", String.Empty, String.Empty)));
                }
                else
                {
                    writer.Write(serializer.Serialize(new WebServiceError(ex.Message, ex.StackTrace, ex.GetType().FullName)));
                }
                writer.Flush();
            }
        }

        #endregion Methods

        #region Nested Types

        internal class WebServiceError
        {
            #region Fields

            public string ExceptionType;
            public string Message;
            public string StackTrace;

            #endregion Fields

            #region Constructors

            public WebServiceError(string msg, string stack, string type)
            {
                Message = msg;
                StackTrace = stack;
                ExceptionType = type;
            }

            #endregion Constructors
        }

        #endregion Nested Types
    }
}