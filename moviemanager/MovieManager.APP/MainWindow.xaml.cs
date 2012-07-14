using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using Common;
using Model;
using MovieManager.APP.Panels;
using System.Windows.Data;
using MovieManager.PLAYER;
using log4net.Core;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace MovieManager.APP
{
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

            foreach (DataGridColumn Column in _videoGrid.Columns)
            {
                _dataGridColumns.Add(Column.Header.ToString(), Column);
            }
            InitVisualColumns();

            _instance = this;
        }

        private void InitVisualColumns()
        {
            var TempColumnsInOrder = new List<DataGridColumn>();
            TempColumnsInOrder.AddRange(_videoGrid.Columns);
            try
            {
                string VisibleColumns = Properties.Settings.Default.VisibleMainViewColumns;

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
                    foreach (DataGridColumn DataGridColumn in _videoGrid.Columns)
                    {
                        VisibleMainViewColumns.Add(new Pair<string, bool>(DataGridColumn.Header.ToString(), true));
                    }
                    ReadSettingsSucceeded = false;
                }

                if (ReadSettingsSucceeded)
                    _videoGrid.Columns.Clear();
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
                        _videoGrid.Columns.Add(_dataGridColumns[ColumnDetails.Key]);

                    CreateAndAddMenuItem(ColumnDetails);
                }
            }
            catch (Exception)
            {
                //if error in settings string --> show all columns
                _videoGrid.Columns.Clear();
                foreach (DataGridColumn Column in TempColumnsInOrder)
                {
                    _videoGrid.Columns.Add(Column);
                    CreateAndAddMenuItem(new Pair<string, bool>(Column.Header.ToString(), true));
                }

            }


            Style HeaderStyle = new Style(typeof(DataGridColumnHeader));
            HeaderStyle.Setters.Add(new Setter
            {
                Property = DataGridColumnHeader.ContextMenuProperty,
                Value = _columnsContextMenu,
            });
            _videoGrid.ColumnHeaderStyle = HeaderStyle;
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
            Item.IsChecked = !((MenuItem)sender).IsChecked;
        }

        void MenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            MenuItem Item = (MenuItem)sender;
            foreach (DataGridColumn Column in _videoGrid.Columns)
            {
                if (Column.Header.ToString() == Item.Header.ToString())
                {
                    _videoGrid.Columns.Remove(Column);
                    break;
                }
            }
        }

        void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            _videoGrid.Columns.Add(_dataGridColumns[((MenuItem)sender).Header.ToString()]);
        }


        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            _controller.FilterEditor = _FilterEditor;
        }

        #region ContextMenu event handlers
        private void MenuItemPropertiesClick(object sender, RoutedEventArgs e)
        {
            Video SelectedVideo = (_videoGrid.SelectedItem as Video);

            VideoEditor Editor = new VideoEditor { DataContext = SelectedVideo };

            Binding VideoBinding = new Binding { Source = SelectedVideo, Path = new PropertyPath("."), Mode = BindingMode.TwoWay };

            Editor.SetBinding(VideoEditor.VIDEO_PROPERTY, VideoBinding);
            Editor.Show();
        }

        private void MenuItemPlayClick(object sender, RoutedEventArgs e)
        {
            var Video = _videoGrid.SelectedItem as Video;
            if (Video != null && Video.Path != null)
            {
                MMPlayer Vlc = new MMPlayer();
                Vlc.Show();
                Vlc.PlayVideo(Video);
            }
        }

        private void MenuItemRenameFileClick(object sender, RoutedEventArgs e)
        {
            if (_videoGrid.SelectedItems != null)
            {
                foreach (Video Video in _videoGrid.SelectedItems)
                {
                    try
                    {
                        string NewVideoName = Path.GetFileName(Video.Path); ;
                        string VideoDir = Path.GetDirectoryName(Video.Path);
                        if (Video is Movie)
                        {
                            string ParString = Properties.Settings.Default.RenamingMovieFileSequence;
                            NewVideoName = ParString.Replace("{{MovieName}}", Video.Name);
                            NewVideoName = NewVideoName.Replace("{{Year}}", Video.Release.Year.ToString());
                        }

                        else if (Video is Episode)
                        {
                            string ParString = Properties.Settings.Default.RenamingEpisodeFileSequence;
                            //TODO 030: implement renaming for episodes
                        }

                        if (!string.IsNullOrEmpty(VideoDir) && File.Exists(Video.Path) && Directory.Exists(VideoDir))
                        {
                            string NewPath = Path.Combine(VideoDir, NewVideoName + Path.GetExtension(Video.Path));
                            File.Move(Video.Path, NewPath);
                            Video.Path = NewPath;
                        }

                        _videoGrid.Items.Refresh();
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
            if (Properties.Settings.Default.MediaPlayerPlayOnDoubleClick)
                MenuItemPlayClick(sender, e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //TODO 050 check if all column are checked off and program is restarted -> no crach

            List<Pair<string, bool>> VisibleMainViewColumns = new List<Pair<string, bool>>();
            foreach (MenuItem Item in _columnsContextMenu.Items)
            {
                VisibleMainViewColumns.Add(new Pair<string, bool>(Item.Header.ToString(), Item.IsChecked));
            }

            XmlSerializer Serializer = new XmlSerializer(typeof(List<Pair<string, bool>>));
            StringWriter StringWriter = new StringWriter();
            Serializer.Serialize(StringWriter, VisibleMainViewColumns);
            Properties.Settings.Default.VisibleMainViewColumns = StringWriter.ToString();
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }


    }
}

//TODO 100: Filter Movies
//TODO 090: gezochte folders bijhouden + (automatische refresh in background)
//TODO 070: apparte settingsfile
//TODO 070: verschillende video types: episode, movie, ...
//TODO 050: tmdb search engine
