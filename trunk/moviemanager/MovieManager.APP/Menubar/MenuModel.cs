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

        public static ControlData EmptyVideos
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string Str = "Clear videos";

                    if (!DATA_COLLECTION.ContainsKey(Str))
                    {
                        const string AddVideosToolTipTitle = "Clear videotables";
                        const string AddVideosToolTipDescription = "Clear all videotables";
                        ControlData ButtonData = new ControlData
                                                     {
                                                         Label = Str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/clear.png", UriKind.Relative),
                                                         ToolTipTitle = AddVideosToolTipTitle,
                                                         ToolTipDescription = AddVideosToolTipDescription,
                                                         Command = new EmptyVideosCommand(),
                                                         KeyTip = "",
                                                     };
                        DATA_COLLECTION[Str] = ButtonData;
                    }

                    return DATA_COLLECTION[Str];
                }
            }
        }

        public static ControlData SearchWeb
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string Str = "Search Web";

                    if (!DATA_COLLECTION.ContainsKey(Str))
                    {
                        ControlData ButtonData = new ControlData
                                                     {
                                                         Label = Str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/search.png", UriKind.Relative),
                                                         ToolTipTitle = "Search TMDB",
                                                         ToolTipDescription = "Search TMDB for info",
                                                         Command = new SearchCommand(),
                                                         KeyTip = "",
                                                     };
                        DATA_COLLECTION[Str] = ButtonData;
                    }

                    return DATA_COLLECTION[Str];
                }
            }
        }


        public static ControlData ExitMM
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string Str = "Exit Word";

                    if (!DATA_COLLECTION.ContainsKey(Str))
                    {
                        ControlData ButtonData = new ControlData
                                                     {
                                                         Label = Str,
                                                         SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                                                         Command = ApplicationCommands.Close,
                                                         KeyTip = "X",
                                                     };
                        DATA_COLLECTION[Str] = ButtonData;
                    }

                    return DATA_COLLECTION[Str];
                }
            }
        }
    }
}
