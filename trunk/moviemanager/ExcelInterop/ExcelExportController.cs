using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SQLite;

namespace ExcelInterop
{
    public class ExcelExportController : INotifyPropertyChanged
    {
        private bool _selectAllNone;

        public ExcelExportController()
        {
            _exportProperties = new ObservableCollection<DatabaseMappingItem>();
            List<String> Headers = new List<String>
                                       {
                                           "id",
                                           "id_imdb",
                                           "name",
                                           "release",
                                           "rating",
                                           "rating_imdb",
                                           "genre",
                                           "path",
                                           "last_play_location"
                                       };//TODO 070 get from database or enum
            _exportProperties.Add(new DatabaseMappingItem
                                      {
                                          MMColumn = "Id",
                                          DatabaseColumn = "Id",
                                          Selected = true
                                      });
            _exportProperties.Add(new DatabaseMappingItem
                                      {
                                          MMColumn = "IMDB id",
                                          DatabaseColumn = "id_imdb",
                                          Selected = true
                                      });
            _exportProperties.Add(new DatabaseMappingItem
                                      {
                                          MMColumn = "Name",
                                          DatabaseColumn = "name",
                                          Selected = true
                                      });
            _exportProperties.Add(new DatabaseMappingItem
                                      {
                                          MMColumn = "Release date",
                                          DatabaseColumn = "release",
                                          Selected = true
                                      });
            _exportProperties.Add(new DatabaseMappingItem
                                      {
                                          MMColumn = "IMDB rating",
                                          DatabaseColumn = "rating_imdb",
                                          Selected = true
                                      });
            _exportProperties.Add(new DatabaseMappingItem
                                      {
                                          MMColumn = "Genre",
                                          DatabaseColumn = "genre",
                                          Selected = true
                                      });
            _exportProperties.Add(new DatabaseMappingItem
                                      {
                                          MMColumn = "Video file path",
                                          DatabaseColumn = "path",
                                          Selected = true
                                      });
            _exportProperties.Add(new DatabaseMappingItem
                                      {
                                          MMColumn = "Last play location",
                                          DatabaseColumn = "last_play_location",
                                          Selected = true
                                      });
            _selectAllNone = false;
            ExportFilePath = "";
        }

        private ObservableCollection<DatabaseMappingItem> _exportProperties;
        public ObservableCollection<DatabaseMappingItem> ExportProperties
        {
            get { return _exportProperties; }
            set { _exportProperties = value; }
        }

        private string _exportFilePath;
        public string ExportFilePath
        {
            get { return _exportFilePath; }
            set
            {
                _exportFilePath = value;
                PropChanged("ExportFilePath");
                PropChanged("IsExportEnabled");
            }
        }

        public void PropChanged(string arg)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(arg));
            }
        }

        public void SelectAllNone()
        {
            foreach (DatabaseMappingItem MappingItem in _exportProperties)
            {
                MappingItem.Selected = _selectAllNone;
            }
            _selectAllNone = !_selectAllNone;
        }

        public void Export()
        {
            if (ExportProperties.Any(mappingItem => mappingItem.Selected))
            {
                //export
                Excel.Data2Excel(
                    MMDatabase.GetVideosDataReader(),
                    (IList<string>) (from MappingItem in ExportProperties where MappingItem.Selected select MappingItem.DatabaseColumn), "videos");//TODO 060 check if this still works -> prob a problem with linq expression
            }
            else
            {
                MessageBox.Show("U hebt geen kolommen geselecteerd om te exporteren.");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
