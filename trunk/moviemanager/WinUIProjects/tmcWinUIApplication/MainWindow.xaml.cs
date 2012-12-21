using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using Model;
using MovieManager.APP.Commands;
using MovieManager.APP.Panels;
using System.Windows.Data;
using MovieManager.APP.Properties;
using MovieManager.Common;
using MovieManager.PLAYER;
using Tmc.SystemFrameworks.Log;
using Tmc.WinUI.Application;
using log4net.Core;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Windows.Input;

namespace MovieManager.APP
{
    //TODO 030 get date format from client pc

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainController _controller;
        private readonly ContextMenu _columnsContextMenu = new ContextMenu();
        private readonly Dictionary<string, DataGridColumn> _dataGridColumns = new Dictionary<string, DataGridColumn>();

        private static MainWindow _instance;
        public static MainWindow Instance
        {
            get { return _instance; }
        }
        public MainWindow()
        {
            InitializeComponent();

            _controller = MainController.Instance;
            _controller.WindowInstance = this;

            DataContext = _controller;
            Loaded += MainWindowLoaded;

            foreach (DataGridColumn Column in _videoDetails.Columns)
            {
                _dataGridColumns.Add(Column.Header.ToString(), Column);
            }
            InitVisualColumns();

            _instance = this;
        }

        private void InitVisualColumns()
        {
            var TempColumnsInOrder = new List<DataGridColumn>();
            TempColumnsInOrder.AddRange(_videoDetails.Columns);
            try
            {
                string VisibleColumns = Settings.Default.VisibleMainViewColumns;

                List<Pair<string, bool>> VisibleMainViewColumns = new List<Pair<string, bool>>();
                bool ReadSettingsSucceeded = true;
                try
                {
                    XmlSerializer Serializer = new XmlSerializer(typeof(List<Pair<string, bool>>));
                    VisibleMainViewColumns =
                        (List<Pair<string, bool>>)Serializer.Deserialize(new StringReader(VisibleColumns));
                }
                catch (Exception)
                {
                    foreach (DataGridColumn DataGridColumn in _videoDetails.Columns)
                    {
                        VisibleMainViewColumns.Add(new Pair<string, bool>(DataGridColumn.Header.ToString(), true));
                    }
                    ReadSettingsSucceeded = false;
                }

                if (ReadSettingsSucceeded)
                    _videoDetails.Columns.Clear();
                if (VisibleMainViewColumns.Count == 0)
                {
                    throw new IndexOutOfRangeException();
                }
                foreach (Pair<string, bool> ColumnDetails in VisibleMainViewColumns)
                {
                    //add to datagrid
                    if (string.IsNullOrEmpty(ColumnDetails.Key))
                    {
                        throw new NullReferenceException();
                    }
                    if (ReadSettingsSucceeded && ColumnDetails.Value)
                        _videoDetails.Columns.Add(_dataGridColumns[ColumnDetails.Key]);

                    CreateAndAddMenuItem(ColumnDetails);
                }
            }
            catch (Exception)
            {
                //if error in settings string --> show all columns
                _videoDetails.Columns.Clear();
                foreach (DataGridColumn Column in TempColumnsInOrder)
                {
                    _videoDetails.Columns.Add(Column);
                    CreateAndAddMenuItem(new Pair<string, bool>(Column.Header.ToString(), true));
                }

            }


            Style HeaderStyle = new Style(typeof(DataGridColumnHeader));
            HeaderStyle.Setters.Add(new Setter
            {
                Property = DataGridColumnHeader.ContextMenuProperty,
                Value = _columnsContextMenu,
            });
            _videoDetails.ColumnHeaderStyle = HeaderStyle;
        }

        private void CreateAndAddMenuItem(Pair<string, bool> columnDetails)
        {
            //add to context menu
            MenuItem MenuItem = new MenuItem { Header = columnDetails.Key, IsChecked = columnDetails.Value };
            _columnsContextMenu.Items.Add(MenuItem);
            MenuItem.Click += new RoutedEventHandler(MenuItem_Click);
            MenuItem.Checked += new RoutedEventHandler(MenuItem_Checked);
            MenuItem.Unchecked += new RoutedEventHandler(MenuItem_Unchecked);
        }


        void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Item = (MenuItem)sender;
            if (Item.IsChecked && _videoDetails.Columns.Count > 1 || !Item.IsChecked)
            {
                Item.IsChecked = !Item.IsChecked;
            }
        }

        void MenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            MenuItem Item = (MenuItem)sender;
            foreach (DataGridColumn Column in _videoDetails.Columns)
            {
                if (Column.Header.ToString() == Item.Header.ToString())
                {
                    _videoDetails.Columns.Remove(Column);
                    break;
                }
            }
        }

        void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            _videoDetails.Columns.Add(_dataGridColumns[((MenuItem)sender).Header.ToString()]);
        }


        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            _controller.FilterEditor = _FilterEditor;
        }

        #region ContextMenu event handlers
        private void MenuItemPropertiesClick(object sender, RoutedEventArgs e)
        {
            Video SelectedVideo = (_videoDetails.SelectedItem as Video);

            VideoEditor Editor = new VideoEditor { DataContext = SelectedVideo };

            Binding VideoBinding = new Binding { Source = SelectedVideo, Path = new PropertyPath("."), Mode = BindingMode.TwoWay };

            Editor.SetBinding(VideoEditor.VIDEO_PROPERTY, VideoBinding);
            Editor.Show();
        }

        private void MenuItemPlayClick(object sender, RoutedEventArgs e)
        {
            PlayVideoCommand PlayCommand = new PlayVideoCommand();
            if (_videoIcons.IsVisible)
            {
                PlayCommand.Execute(_videoIcons.SelectedItem);
            }
            else
            {
                PlayCommand.Execute(_videoDetails.SelectedItem);
            }
        }

        private void MenuItemRenameFileClick(object sender, RoutedEventArgs e)
        {
            if (_videoDetails.SelectedItems != null)
            {
                foreach (Video Video in _videoDetails.SelectedItems)
                {
                    try
                    {
                        string NewVideoName = Path.GetFileName(Video.Path);
                        string VideoDir = Path.GetDirectoryName(Video.Path);
                        if (Video is Movie)
                        {
                            string ParString = Settings.Default.RenamingMovieFileSequence;
                            NewVideoName = ParString.Replace("{{MovieName}}", Video.Name);
                            NewVideoName = NewVideoName.Replace("{{Year}}", Video.Release.Year.ToString());
                        }

                        else if (Video is Episode)
                        {
                            string ParString = Settings.Default.RenamingEpisodeFileSequence;
                            //TODO 030: implement renaming for episodes
                        }

                        if (!string.IsNullOrEmpty(VideoDir) && File.Exists(Video.Path) && Directory.Exists(VideoDir))
                        {
                            string NewPath = Path.Combine(VideoDir, NewVideoName + Path.GetExtension(Video.Path));
                            File.Move(Video.Path, NewPath);
                            Video.Path = NewPath;
                        }

                        _videoDetails.Items.Refresh();
                    }
                    catch (Exception ex)
                    {
                        GlobalLogger.Instance.MovieManagerLogger.Error(GlobalLogger.FormatExceptionForLog("MainWindow", "MenuItemRenameFileClick", ex.Message));
                    }
                }
            }
        }

        #endregion

        private void VideoGridMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Settings.Default.MediaPlayerPlayOnDoubleClick)
                MenuItemPlayClick(sender, e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            List<Pair<string, bool>> VisibleMainViewColumns = new List<Pair<string, bool>>();
            bool OneColumnVisible = false;
            foreach (MenuItem Item in _columnsContextMenu.Items)
            {
                if (!OneColumnVisible & Item.IsChecked)
                    OneColumnVisible = true;
                VisibleMainViewColumns.Add(new Pair<string, bool>(Item.Header.ToString(), Item.IsChecked));
            }

            if (!OneColumnVisible)
                VisibleMainViewColumns[0].Value = true;

            XmlSerializer Serializer = new XmlSerializer(typeof(List<Pair<string, bool>>));
            StringWriter StringWriter = new StringWriter();
            Serializer.Serialize(StringWriter, VisibleMainViewColumns);
            Settings.Default.VisibleMainViewColumns = StringWriter.ToString();
            Settings.Default.Save();

            base.OnClosing(e);
        }

        private void Window_PreviewMouseWheel_1(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                _controller.Zoom(e.Delta > 0);
        }

        private new void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                e.Handled = true;
        }
    }
}

//TODO 090: gezochte folders bijhouden + (automatische refresh in background)
//TODO 070: apparte settingsfile
//TODO 070: verschillende video types: episode, movie, ...
