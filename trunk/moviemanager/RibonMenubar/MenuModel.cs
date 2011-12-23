using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Windows.Controls.Ribbon;

namespace RibonMenubar
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
                    string Str = "AddVideos";

                    if (!_dataCollection.ContainsKey(Str))
                    {
                        string AddVideosToolTipTitle = "Add Videos";
                        string AddVideosToolTipDescription = "Add Videos To Database";
                        ControlData buttonData = new ControlData()
                        {
                            Label = Str,
                            SmallImage = new Uri("/RibonMenubar;component/Images/add.png", UriKind.Relative),
                            ToolTipTitle = AddVideosToolTipTitle,
                            ToolTipDescription = AddVideosToolTipDescription,
                            Command = ApplicationCommands.Cut,
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
                            SmallImage = new Uri("/RibonMenubar;component/Images/add.png", UriKind.Relative),
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
