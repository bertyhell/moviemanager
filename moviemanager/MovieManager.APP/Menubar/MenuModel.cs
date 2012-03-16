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
                    const string Str = "Add Files";

                    if (!DATA_COLLECTION.ContainsKey(Str))
                    {
                        const string AddVideosToolTipTitle = "Add Video Files";
                        const string AddVideosToolTipDescription = "Add Videos To Database";
                        ControlData ButtonData = new ControlData
                                                     {
                                                         Label = Str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                                                         ToolTipTitle = AddVideosToolTipTitle,
                                                         ToolTipDescription = AddVideosToolTipDescription,
                                                         Command = new AddVideosCommand(),
                                                         KeyTip = "",
                                                     };
                        DATA_COLLECTION[Str] = ButtonData;
                    }

                    return DATA_COLLECTION[Str];
                }
            }
        }




        public static ControlData AddVideoDir
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string Str = "Add Directory";

                    if (!DATA_COLLECTION.ContainsKey(Str))
                    {
                        const string AddVideosToolTipTitle = "Add Video Directory";
                        const string AddVideosToolTipDescription = "Add Videos To Database";
                        ControlData ButtonData = new ControlData
                                                     {
                                                         Label = Str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                                                         ToolTipTitle = AddVideosToolTipTitle,
                                                         ToolTipDescription = AddVideosToolTipDescription,
                                                         Command = new AddVideosDirectoryCommand(),
                                                         KeyTip = "",
                                                     };
                        DATA_COLLECTION[Str] = ButtonData;
                    }

                    return DATA_COLLECTION[Str];
                }
            }
        }

        public static ControlData AddSerie
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string Str = "Add Serie";

                    if (!DATA_COLLECTION.ContainsKey(Str))
                    {
                        const string AddSerieToolTipTitle = "Add Serie Directory";
                        const string AddSerieToolTipDescription = "Add Serie To Database";
                        ControlData ButtonData = new ControlData
                        {
                            Label = Str,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                            ToolTipTitle = AddSerieToolTipTitle,
                            ToolTipDescription = AddSerieToolTipDescription,
                            Command = new AddSerieCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[Str] = ButtonData;
                    }

                    return DATA_COLLECTION[Str];
                }
            }
        }

        public static ControlData EmptyVideos
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string str = "Clear videos";

                    if (!DATA_COLLECTION.ContainsKey(str))
                    {
                        const string addVideosToolTipTitle = "Clear videotables";
                        const string addVideosToolTipDescription = "Clear all videotables";
                        ControlData buttonData = new ControlData
                                                     {
                                                         Label = str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/clear.png", UriKind.Relative),
                                                         ToolTipTitle = addVideosToolTipTitle,
                                                         ToolTipDescription = addVideosToolTipDescription,
                                                         Command = new EmptyVideosCommand(),
                                                         KeyTip = "",
                                                     };
                        DATA_COLLECTION[str] = buttonData;
                    }

                    return DATA_COLLECTION[str];
                }
            }
        }

        public static ControlData ExportVideos
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string str = "Export videos";

                    if (!DATA_COLLECTION.ContainsKey(str))
                    {
                        const string addVideosToolTipTitle = "Export all videos to Excel";
                        const string addVideosToolTipDescription = "Export all videos to Excel";
                        ControlData buttonData = new ControlData
                        {
                            Label = str,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/excel_export_32.png", UriKind.Relative),
                            ToolTipTitle = addVideosToolTipTitle,
                            ToolTipDescription = addVideosToolTipDescription,
                            Command = new ExportVideosCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[str] = buttonData;
                    }

                    return DATA_COLLECTION[str];
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
                    const string str = "Search Web";

                    if (!DATA_COLLECTION.ContainsKey(str))
                    {
                        ControlData buttonData = new ControlData
                                                     {
                                                         Label = str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/search.png", UriKind.Relative),
                                                         ToolTipTitle = "Search TMDB",
                                                         ToolTipDescription = "Search TMDB for info",
                                                         Command = new SearchCommand(),
                                                         KeyTip = "",
                                                     };
                        DATA_COLLECTION[str] = buttonData;
                    }

                    return DATA_COLLECTION[str];
                }
            }
        }

        public static ControlData Analyse
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string str = "Analyse";

                    if (!DATA_COLLECTION.ContainsKey(str))
                    {
                        ControlData buttonData = new ControlData
                        {
                            Label = str,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/analyse.png", UriKind.Relative),
                            ToolTipTitle = "Analyse videos",
                            ToolTipDescription = "Analyse videos and search info on the web",
                            Command = new AnalyseCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[str] = buttonData;
                    }

                    return DATA_COLLECTION[str];
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
                    const string str = "Exit MovieManager";

                    if (!DATA_COLLECTION.ContainsKey(str))
                    {
                        ControlData buttonData = new ControlData
                                                     {
                                                         Label = str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                                                         Command = ApplicationCommands.Close,
                                                         KeyTip = "X",
                                                     };
                        DATA_COLLECTION[str] = buttonData;
                    }

                    return DATA_COLLECTION[str];
                }
            }
        }

        public static ControlData EditEpisodeRegexSettings
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string Str = "Edit Episode Regex";

                    if (!DATA_COLLECTION.ContainsKey(Str))
                    {
                        const string ToolTipTitle = "Edit Episode Regex";
                        const string ToolTipDescription = "Edit Episode Regex";
                        ControlData ButtonData = new ControlData
                        {
                            Label = Str,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/RegExp.gif", UriKind.Relative),
                            ToolTipTitle = ToolTipTitle,
                            ToolTipDescription = ToolTipDescription,
                            Command = new EditEpisodeRegExCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[Str] = ButtonData;
                    }

                    return DATA_COLLECTION[Str];
                }
            }
        }
    }
}
