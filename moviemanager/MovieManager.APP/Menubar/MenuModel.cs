using System;
using System.Collections.Generic;
using System.Windows.Input;
using MovieManager.APP.Commands;

namespace MovieManager.APP.Menubar
{
    public static class MenuModel
    {


        #region Data

        private static readonly object LockObject = new object();
        private static readonly Dictionary<string, ControlData> DataCollection = new Dictionary<string, ControlData>();

        #endregion Data


        public static ControlData AddVideo
        {
            get
            {
                lock (LockObject)
                {
                    const string str = "Add Files";

                    if (!DataCollection.ContainsKey(str))
                    {
                        const string addVideosToolTipTitle = "Add Video Files";
                        const string addVideosToolTipDescription = "Add Videos To Database";
                        ControlData buttonData = new ControlData
                                                     {
                                                         Label = str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                                                         ToolTipTitle = addVideosToolTipTitle,
                                                         ToolTipDescription = addVideosToolTipDescription,
                                                         Command = new AddVideosCommand(),
                                                         KeyTip = "",
                                                     };
                        DataCollection[str] = buttonData;
                    }

                    return DataCollection[str];
                }
            }
        }




        public static ControlData AddVideoDir
        {
            get
            {
                lock (LockObject)
                {
                    const string str = "Add Directory";

                    if (!DataCollection.ContainsKey(str))
                    {
                        const string addVideosToolTipTitle = "Add Video Directory";
                        const string addVideosToolTipDescription = "Add Videos To Database";
                        ControlData buttonData = new ControlData
                                                     {
                                                         Label = str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                                                         ToolTipTitle = addVideosToolTipTitle,
                                                         ToolTipDescription = addVideosToolTipDescription,
                                                         Command = new AddVideosDirectoryCommand(),
                                                         KeyTip = "",
                                                     };
                        DataCollection[str] = buttonData;
                    }

                    return DataCollection[str];
                }
            }
        }

        public static ControlData EmptyVideos
        {
            get
            {
                lock (LockObject)
                {
                    const string str = "Clear videos";

                    if (!DataCollection.ContainsKey(str))
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
                        DataCollection[str] = buttonData;
                    }

                    return DataCollection[str];
                }
            }
        }

        public static ControlData ExportVideos
        {
            get
            {
                lock (LockObject)
                {
                    const string str = "Export videos";

                    if (!DataCollection.ContainsKey(str))
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
                        DataCollection[str] = buttonData;
                    }

                    return DataCollection[str];
                }
            }
        }

        #region web

        public static ControlData SearchWeb
        {
            get
            {
                lock (LockObject)
                {
                    const string str = "Search Web";

                    if (!DataCollection.ContainsKey(str))
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
                        DataCollection[str] = buttonData;
                    }

                    return DataCollection[str];
                }
            }
        }

        public static ControlData Analyse
        {
            get
            {
                lock (LockObject)
                {
                    const string str = "Analyse";

                    if (!DataCollection.ContainsKey(str))
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
                        DataCollection[str] = buttonData;
                    }

                    return DataCollection[str];
                }
            }
        }
        #endregion

        public static ControlData ExitMM
        {
            get
            {
                lock (LockObject)
                {
                    const string str = "Exit MovieManager";

                    if (!DataCollection.ContainsKey(str))
                    {
                        ControlData buttonData = new ControlData
                                                     {
                                                         Label = str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                                                         Command = ApplicationCommands.Close,
                                                         KeyTip = "X",
                                                     };
                        DataCollection[str] = buttonData;
                    }

                    return DataCollection[str];
                }
            }
        }
    }
}
