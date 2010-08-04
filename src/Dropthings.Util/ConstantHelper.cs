using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Dropthings.Util
{
    public class ConstantHelper
    {
        public static bool SetupCompleted = false;

        public const string DEFAULT_CONNECTION_STRING = "DropthingsConnectionString";
        public const string ENTITY_FRAMEWORK_CONNECTION_STRING = "DropthingsDataContext";
        public const string SETUP_COMPLETE_FILE = "~/App_Data/SetupComplete.txt";

        public static readonly bool DeveloperMode = Convert.ToBoolean(ConfigurationManager.AppSettings["DeveloperMode"] ?? "false");
        public static readonly string ScriptVersionNo = ConfigurationManager.AppSettings["ScriptVersionNo"] ?? string.Empty;
        public static readonly string CssVersionNo = ConfigurationManager.AppSettings["CssVersionNo"] ?? string.Empty;
        public readonly static string ImagePrefix = ConfigurationManager.AppSettings["ImgPrefix"] ?? string.Empty;
        public readonly static string ScriptPrefix = ConfigurationManager.AppSettings["JsPrefix"] ?? string.Empty;
        public readonly static string CssPrefix = ConfigurationManager.AppSettings["CssPrefix"] ?? string.Empty;
        public readonly static string CommonCssFileName = System.Configuration.ConfigurationManager.AppSettings["CommonCssSet"] ?? string.Empty;
        public readonly static bool EnableTabSorting = bool.Parse(ConfigurationManager.AppSettings["EnableTabSorting"] ?? "false");
        public readonly static bool EnableAdminonlyTabSorting = bool.Parse(ConfigurationManager.AppSettings["EnableAdminOnlyTabSorting"] ?? "false");
        public readonly static bool ActivationRequired = Convert.ToBoolean(ConfigurationManager.AppSettings["ActivationRequired"] ?? "false");
        public readonly static bool EnableWidgetPermission = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableWidgetPermission"] ?? "false");
        public static readonly bool DisableDOSCheck = bool.Parse(ConfigurationManager.AppSettings["DisableDOSCheck"] ?? "false");
        public static readonly string AdminEmail = ConfigurationManager.AppSettings["AdminEmail"] ?? string.Empty;
        public static readonly string WebRoot = ConfigurationManager.AppSettings["WebRoot"] ?? string.Empty;
        public static readonly bool DisableCache = Convert.ToBoolean(ConfigurationManager.AppSettings["DisableCache"] ?? "false");
        public static readonly bool EnableVelocity = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableVelocity"] ?? "false");
        public static readonly string VelocityCacheName = ConfigurationManager.AppSettings["VelocityCacheName"] ?? "dropthings";
    }
}
