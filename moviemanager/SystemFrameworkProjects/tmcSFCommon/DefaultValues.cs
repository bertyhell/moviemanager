using System;
using System.IO;

namespace Tmc.SystemFrameworks.Common
{
    public class DefaultValues
    {
        public static readonly DateTime NULL_DATE = new DateTime(1900, 1, 1);
        public static readonly DateTime FILTER_START_DATE = DateTime.Now.AddYears(-2);
        public static readonly DateTime FILTER_END_DATE = DateTime.Now;

        public static readonly string PATH_USER_APPDATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string PATH_PROGRAM_DATA = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static readonly string PATH_LOGGING_SUBDIR = "Log";
        public static readonly string PATH_SETTINGS_SUBDIR = "Settings";

        public static readonly string PATH_APPLICATION_SUBDIR = "TheMovieCollector";

        public static readonly string LOG_FILENAME = "moviemanager_log.sqlite";

        public static readonly int PREVIEW_MIN_WIDTH = 60;
        public static readonly int PREVIEW_MAX_WIDTH = 500;
        public static readonly int PREVIEW_ZOOM_STEP = 30;

        public static string LogDirectory
        {
            get { return Path.Combine(PATH_PROGRAM_DATA, PATH_APPLICATION_SUBDIR, PATH_LOGGING_SUBDIR); }
        }
    }
}
