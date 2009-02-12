namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    using Dropthings.Test.Plugin;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    internal static class RuleHelper
    {
        #region Fields

        private static Regex _FindContextKey = new Regex(@"{{(.*?)}}", 
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        #endregion Fields

        #region Methods

        public static Cookie GetCookie(string name, string domain, CookieCollection cookies)
        {
            // Same cookie can be issued on different domains. So, we need to check if there's any domain expected
            // and if domain is specified, get the cookie for that domain
            if (!string.IsNullOrEmpty(domain))
            {
                // Find the cookir for the specific domain
                foreach (Cookie cookie in cookies)
                {
                    if (cookie.Name == name && cookie.Domain == domain)
                        return cookie;
                }

                // Cookie not found for the specified domain
                return null;
            }
            else
            {
                // No domain specified, so the first cookie that matches the domain will be sent
                return cookies[name];
            }
        }

        public static bool NotAlreadyDone(WebTestContext context, string stepKey, Action callback)
        {
            int stepNo = (int)context[ASPNETWebTestPlugin.STEP_NO_KEY];
            string keyName = stepKey + "." + stepNo;
            if (context.ContainsKey(keyName))
            {
                return false;
            }
            else
            {
                callback();
                context[keyName] = true;
                return true;
            }
        }

        public static void NotAlreadyExtracted<TExtractionRule>(ExtractionRuleReferenceCollection rules, Action callback)
            where TExtractionRule : ExtractionRule
        {
            bool found = false;
            foreach (ExtractionRuleReference ruleRef in rules)
            {
                if (ruleRef.Type == typeof(TExtractionRule))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                callback();
        }

        public static string PlaceUniqueItem(WebTestContext context, string keyPrefix, object value)
        {
            int i = 1;
            while (context.ContainsKey(keyPrefix + "." + i))
            {
                if (((IComparable)context[keyPrefix + "." + i]).Equals(value))
                    break; // We already have this exact control in Context.
                else
                    i++; // Another instance of the same Control. Try another number in Key.
            }

            string keyName = keyPrefix + "." + i;
            context[keyName] = value;

            return keyName;
        }

        public static string ResolveContextValue(WebTestContext context, string value)
        {
            return _FindContextKey.Replace(value, new MatchEvaluator((m) => {
                string keyName = m.Groups[1].Value;
                if (context.ContainsKey(keyName))
                    return context[keyName].ToString();
                else
                    throw new ApplicationException(string.Format("Key {0} not found in Context", keyName));
            }));
        }

        public static void WhenAspNetResponse(WebTestResponse response, Action callback)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (response.ContentType.Contains("text/html")
                    || response.ContentType.Contains("text/plain"))
                {
                    callback();
                }
            }
        }

        #endregion Methods
    }
}