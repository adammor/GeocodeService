using System;
using System.IO;
using System.Reflection;

namespace RestEL.Modules
{
    public static class Constants
    {
        private static readonly string SETTINGS_FILE_PATH = @".\";
        private static readonly string SETTINGS_DIRECTORY = @"settings";
        private static readonly string SETTINGS_SERVICEAPI_FILE = @"serviceapi.json";
        private static readonly string SETTINGS_PACKAGE_FILE = @"package.json";

        public static readonly string SERVICEAPI_FILE = Path.Combine(SETTINGS_FILE_PATH, SETTINGS_DIRECTORY, SETTINGS_SERVICEAPI_FILE);
        public static readonly string PACKAGE_FILE = Path.Combine(SETTINGS_FILE_PATH, SETTINGS_DIRECTORY, SETTINGS_PACKAGE_FILE);

        public static readonly string LIB_SERVICE_NAME = Assembly.GetExecutingAssembly().GetName().Name;   // autodetect this namespace

        //constants
        public const int MILLISECONDS = 1000;
    }
}
