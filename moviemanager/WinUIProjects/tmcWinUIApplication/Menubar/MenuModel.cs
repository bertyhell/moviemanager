using System;
using System.Collections.Generic;
using System.Windows.Input;
using Tmc.WinUI.Application.Commands;
using Tmc.WinUI.Application.Commands.Debug;

namespace Tmc.WinUI.Application.Menubar
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
                                                         SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/add.png", UriKind.Relative),
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
                                                         SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/add.png", UriKind.Relative),
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
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/add.png", UriKind.Relative),
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
                                                         SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/clear.png", UriKind.Relative),
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
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/excel_export_32.png", UriKind.Relative),
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

        public static ControlData ImportVideos
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Import videos";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string ADD_VIDEOS_TOOL_TIP_TITLE = "Import videos from Excel";
                        const string ADD_VIDEOS_TOOL_TIP_DESCRIPTION = "Import videos from Excel";
                        ControlData ButtonData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/excel_import_32.png", UriKind.Relative),
                            ToolTipTitle = ADD_VIDEOS_TOOL_TIP_TITLE,
                            ToolTipDescription = ADD_VIDEOS_TOOL_TIP_DESCRIPTION,
                            Command = new ImportVideosCommand(),
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
                                                         SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/search.png", UriKind.Relative),
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
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/analyse.png", UriKind.Relative),
                            ToolTipTitle = "Analyse videos",
                            ToolTipDescription = "Analyse videos and search info on the web",
                            Command = new AnalyseCommand(),
                            KeyTip = ""
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
                                                         SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/add.png", UriKind.Relative),
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
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/RegExp.gif", UriKind.Relative),
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
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/settings_32.png", UriKind.Relative),
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

        public static ControlData Logging
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Logging";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string LOGGING_TOOL_TIP_TITLE = "Logging";
                        const string LOGGING_TOOL_TIP_DESCRIPTION = "Logging";
                        ControlData ButtonData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/log_32.png", UriKind.Relative),
                            ToolTipTitle = LOGGING_TOOL_TIP_TITLE,
                            ToolTipDescription = LOGGING_TOOL_TIP_DESCRIPTION,
                            Command = new LoggingToolCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData TogglePreviewTitle
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Toggle Title";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        ControlData ButtonData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/header_32.png", UriKind.Relative),
                            ToolTipTitle = "Toggle Title",
                            ToolTipDescription = "Toggle Title",
                            Command = new TogglePreviewTitleCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData TogglePreviewMargin
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Toggle Margin";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string VIEW_TOOL_TIP_TITLE = "Toggle Margin";
                        const string VIEW_TOOL_TIP_DESCRIPTION = "Toggle Margin";

                        ControlData ButtonData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/margin_32.png", UriKind.Relative),
                            ToolTipTitle = VIEW_TOOL_TIP_TITLE,
                            ToolTipDescription = VIEW_TOOL_TIP_DESCRIPTION,
                            Command = new TogglePreviewMarginCommand(),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData ChangeView //TODO 070 check why the **** this doesn't work :@ (wasted 3 hours)
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Change View";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string VIEW_TOOL_TIP_TITLE = "Change View";
                        const string VIEW_TOOL_TIP_DESCRIPTION = "Change view";

                        ControlData MenuButtonData = new ControlData
                        {
                            Label = STR,
                            LargeImage = new Uri("/Tmc.WinUI.Application;component/Images/view_32.png", UriKind.Relative),
                            ToolTipTitle = VIEW_TOOL_TIP_TITLE,
                            ToolTipDescription = VIEW_TOOL_TIP_DESCRIPTION,
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = MenuButtonData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData ViewBigIcons
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Big icons";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string VIEW_TOOL_TIP_TITLE = "Big icons";
                        const string VIEW_TOOL_TIP_DESCRIPTION = "Change view to big icons";
                        ControlData ItemData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/view_big_icons_16.png", UriKind.Relative),
                            ToolTipTitle = VIEW_TOOL_TIP_TITLE,
                            ToolTipDescription = VIEW_TOOL_TIP_DESCRIPTION,
                            Command = new ChangeViewCommand(ViewStates.BigIcons),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ItemData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData ViewMediumIcons
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Medium icons";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string VIEW_TOOL_TIP_TITLE = "Medium icons";
                        const string VIEW_TOOL_TIP_DESCRIPTION = "Change view to medium icons";
                        ControlData ItemData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/view_medium_icons_16.png", UriKind.Relative),
                            ToolTipTitle = VIEW_TOOL_TIP_TITLE,
                            ToolTipDescription = VIEW_TOOL_TIP_DESCRIPTION,
                            Command = new ChangeViewCommand(ViewStates.MediumIcons),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ItemData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData ViewSmallIcons
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Small icons";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string VIEW_TOOL_TIP_TITLE = "Small icons";
                        const string VIEW_TOOL_TIP_DESCRIPTION = "Change view to small icons";
                        ControlData ItemData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/view_small_icons_16.png", UriKind.Relative),
                            ToolTipTitle = VIEW_TOOL_TIP_TITLE,
                            ToolTipDescription = VIEW_TOOL_TIP_DESCRIPTION,
                            Command = new ChangeViewCommand(ViewStates.SmallIcons),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ItemData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }

        public static ControlData ViewDetails
        {
            get
            {
                lock (LOCK_OBJECT)
                {
                    const string STR = "Details";

                    if (!DATA_COLLECTION.ContainsKey(STR))
                    {
                        const string VIEW_TOOL_TIP_TITLE = "Details";
                        const string VIEW_TOOL_TIP_DESCRIPTION = "Change view to show details";
                        ControlData ItemData = new ControlData
                        {
                            Label = STR,
                            SmallImage = new Uri("/Tmc.WinUI.Application;component/Images/view_details_16.png", UriKind.Relative),
                            ToolTipTitle = VIEW_TOOL_TIP_TITLE,
                            ToolTipDescription = VIEW_TOOL_TIP_DESCRIPTION,
                            Command = new ChangeViewCommand(ViewStates.Details),
                            KeyTip = "",
                        };
                        DATA_COLLECTION[STR] = ItemData;
                    }

                    return DATA_COLLECTION[STR];
                }
            }
        }
    }
}
