using System;
using System.Collections.Generic;
using System.Windows.Input;
using MovieManager.APP.Commands;

namespace MovieManager.APP.Menubar
{
    public static class MenuModel
    {


        #region Data

        private static readonly object LOCK_OBJECT = new object();
        private static readonly Dictionary<string, ControlData> DATA_COLLECTION = new Dictionary<string, ControlData>();

        #endregion Data


        public static ControlData AddVideo
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Add Files";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string ADD_VIDEOS_TOOL_TIP_TITLE = "Add Video Files";
                        const string ADD_VIDEOS_TOOL_TIP_DESCRIPTION = "Add Videos To Database";
                        ControlData ButtonData = new ControlData
                                                     {
                                                         Label = STR,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                                                         ToolTipTitle = ADD_VIDEOS_TOOL_TIP_TITLE,
                                                         ToolTipDescription = ADD_VIDEOS_TOOL_TIP_DESCRIPTION,
                                                         Command = new AddVideosCommand(),
                                                         KeyTip = "",
                                                     };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }




        public static ControlData AddVideoDir
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Add Directory";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string ADD_VIDEOS_TOOL_TIP_TITLE = "Add Video Directory";
                        const string ADD_VIDEOS_TOOL_TIP_DESCRIPTION = "Add Videos To Database";
                        ControlData ButtonData = new ControlData
                                                     {
                                                         Label = STR,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                                                         ToolTipTitle = ADD_VIDEOS_TOOL_TIP_TITLE,
                                                         ToolTipDescription = ADD_VIDEOS_TOOL_TIP_DESCRIPTION,
                                                         Command = new AddVideosDirectoryCommand(),
                                                         KeyTip = "",
                                                     };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData AddSerie
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Add Serie";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string ADD_SERIE_TOOL_TIP_TITLE = "Add Serie Directory";
                        const string ADD_SERIE_TOOL_TIP_DESCRIPTION = "Add Serie To Database";
                        ControlData ButtonData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                            ToolTipTitle = ADD_SERIE_TOOL_TIP_TITLE,
                            ToolTipDescription = ADD_SERIE_TOOL_TIP_DESCRIPTION,
                            Command = new AddSerieCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData EmptyVideos
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Clear videos";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string ADD_VIDEOS_TOOL_TIP_TITLE = "Clear videotables";
                        const string ADD_VIDEOS_TOOL_TIP_DESCRIPTION = "Clear all videotables";
                        ControlData ButtonData = new ControlData
                                                     {
                                                         Label = STR,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/clear.png", UriKind.Relative),
                                                         ToolTipTitle = ADD_VIDEOS_TOOL_TIP_TITLE,
                                                         ToolTipDescription = ADD_VIDEOS_TOOL_TIP_DESCRIPTION,
                                                         Command = new EmptyVideosCommand(),
                                                         KeyTip = "",
                                                     };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData ExportVideos
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Export videos";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string ADD_VIDEOS_TOOL_TIP_TITLE = "Export all videos to Excel";
                        const string ADD_VIDEOS_TOOL_TIP_DESCRIPTION = "Export all videos to Excel";
                        ControlData ButtonData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/excel_export_32.png", UriKind.Relative),
                            ToolTipTitle = ADD_VIDEOS_TOOL_TIP_TITLE,
                            ToolTipDescription = ADD_VIDEOS_TOOL_TIP_DESCRIPTION,
                            Command = new ExportVideosCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        #region web

        public static ControlData SearchWeb
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Search Web";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        ControlData ButtonData = new ControlData
                                                     {
                                                         Label = STR,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/search.png", UriKind.Relative),
                                                         ToolTipTitle = "Search TMDB",
                                                         ToolTipDescription = "Search TMDB for info",
                                                         Command = new SearchCommand(),
                                                         KeyTip = "",
                                                     };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData Analyse
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Analyse";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        ControlData ButtonData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/analyse.png", UriKind.Relative),
                            ToolTipTitle = "Analyse videos",
                            ToolTipDescription = "Analyse videos and search info on the web",
                            Command = new AnalyseCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }
        #endregion

        public static ControlData ExitMM
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Exit MovieManager";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        ControlData ButtonData = new ControlData
                                                     {
                                                         Label = STR,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                                                         Command = ApplicationCommands.Close,
                                                         KeyTip = "X",
                                                     };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData EditEpisodeRegexSettings
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Edit Episode Regex";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string TOOL_TIP_TITLE = "Edit Episode Regex";
                        const string TOOL_TIP_DESCRIPTION = "Edit Episode Regex";
                        ControlData ButtonData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/RegExp.gif", UriKind.Relative),
                            ToolTipTitle = TOOL_TIP_TITLE,
                            ToolTipDescription = TOOL_TIP_DESCRIPTION,
                            Command = new EditEpisodeRegExCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData EditSettings
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Edit Settings";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string TOOL_TIP_TITLE = "Edit Settings";
                        const string TOOL_TIP_DESCRIPTION = "Edit Settings";
                        ControlData ButtonData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/settings_32.png", UriKind.Relative),
                            ToolTipTitle = TOOL_TIP_TITLE,
                            ToolTipDescription = TOOL_TIP_DESCRIPTION,
                            Command = new EditSettingsCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }
    }
}
