using System;
using System.IO;

namespace MovieManager.APP.Common
{
    class DefaultValues
    {
        public static readonly DateTime NULL_DATE = new DateTime(1900,1,1);
        public static readonly DateTime FILTER_START_DATE = DateTime.Now.AddYears(-2);
        public static readonly DateTime FILTER_END_DATE = DateTime.Now.AddYears(-10);

        public static readonly string PATH_PROGRAM_DATA = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static readonly string PATH_LOGGING_SUBDIR = "Log";

        public static readonly string PATH_APPLICATION_SUBDIR = "MovieManager";

        public static readonly string LOG_FILENAME = "moviemanager_log.sqlite";

        public static string LogDirectory
        {
            get { return Path.Combine(PATH_PROGRAM_DATA, PATH_APPLICATION_SUBDIR, PATH_LOGGING_SUBDIR); }
        }
    }
}
