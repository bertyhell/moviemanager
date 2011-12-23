using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Windows.Controls.Ribbon;
using SQLite.Commands;

namespace MovieManager.APP.Menubar
{
    public static class MenuModel
    {


        #region Data

        private const string HelpFooterTitle = "Press F1 for more help.";
        private static object _lockObject = new object();
        private static Dictionary<string, ControlData> _dataCollection = new Dictionary<string, ControlData>();

        #endregion Data


        public static ControlData AddVideo
        {
            get
            {
                lock (_lockObject)
                {
                    string Str = "Add Files";

                    if (!_dataCollection.ContainsKey(Str))
                    {
                        string AddVideosToolTipTitle = "Add Video Files";
                        string AddVideosToolTipDescription = "Add Videos To Database";
                        ControlData buttonData = new ControlData()
                        {
                            Label = Str,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                            ToolTipTitle = AddVideosToolTipTitle,
                            ToolTipDescription = AddVideosToolTipDescription,
                            Command = new AddVideosCommand(),
                            KeyTip = "",
                        };
                        _dataCollection[Str] = buttonData;
                    }

                    return _dataCollection[Str];
                }
            }
        }




        public static ControlData AddVideoDir
        {
            get
            {
                lock (_lockObject)
                {
                    string Str = "Add Directory";

                    if (!_dataCollection.ContainsKey(Str))
                    {
                        string AddVideosToolTipTitle = "Add Video Directory";
                        string AddVideosToolTipDescription = "Add Videos To Database";
                        ControlData buttonData = new ControlData()
                        {
                            Label = Str,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                            ToolTipTitle = AddVideosToolTipTitle,
                            ToolTipDescription = AddVideosToolTipDescription,
                            Command = new AddVideosDirectoryCommand(),
                            KeyTip = "",
                        };
                        _dataCollection[Str] = buttonData;
                    }

                    return _dataCollection[Str];
                }
            }
        }

        public static ControlData EmptyVideos
        {
            get
            {
                lock (_lockObject)
                {
                    string Str = "Clear videos";

                    if (!_dataCollection.ContainsKey(Str))
                    {
                        string AddVideosToolTipTitle = "Clear videotables";
                        string AddVideosToolTipDescription = "Clear all videotables";
                        ControlData buttonData = new ControlData()
                        {
                            Label = Str,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/clear.png", UriKind.Relative),
                            ToolTipTitle = AddVideosToolTipTitle,
                            ToolTipDescription = AddVideosToolTipDescription,
                            Command = new EmptyVideosCommand(),
                            KeyTip = "",
                        };
                        _dataCollection[Str] = buttonData;
                    }

                    return _dataCollection[Str];
                }
            }
        }

        public static ControlData SearchWeb
        {
            get
            {
                lock (_lockObject)
                {
                    string Str = "Search Web";

                    if (!_dataCollection.ContainsKey(Str))
                    {
                        ControlData buttonData = new ControlData()
                        {
                            Label = Str,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/search.png", UriKind.Relative),
                            ToolTipTitle = "Search TMDB",
                            ToolTipDescription = "Search TMDB for info",
                            Command =  new MovieManager.APP.Search.SearchCommand(),
                            KeyTip = "",
                        };
                        _dataCollection[Str] = buttonData;
                    }

                    return _dataCollection[Str];
                }
            }
        }


        public static ControlData ExitMM
        {
            get
            {
                lock (_lockObject)
                {
                    string Str = "Exit Word";

                    if (!_dataCollection.ContainsKey(Str))
                    {
                        ControlData buttonData = new ControlData()
                        {
                            Label = Str,
                            SmallImage = new Uri("/MovieManager.APP;component/Images/add.png", UriKind.Relative),
                            Command = ApplicationCommands.Close,
                            KeyTip = "X",
                        };
                        _dataCollection[Str] = buttonData;
                    }

                    return _dataCollection[Str];
                }
            }
        }
    }
}
