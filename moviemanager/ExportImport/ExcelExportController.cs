using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExcelInterop;
using Model;
using SQLite;

namespace ExportImport
{
    public class ExcelExportController : INotifyPropertyChanged
    {
        private bool _selectAllNone;

        public ExcelExportController()
        {
            _exportProperties = new ObservableCollection<ObjectMappingItem>();
            foreach (var Prop in Video.ExportableProperties)
            {
                _exportProperties.Add(new ObjectMappingItem
                {
                    MMColumn = Prop,
                    ObjectProperty = Prop,
                    Selected = true
                });
            }
            _selectAllNone = false;
            ExportFilePath = Path.Combine(Path.GetTempPath(), "exportedVideos.xls");
        }

        private ObservableCollection<ObjectMappingItem> _exportProperties;
        public ObservableCollection<ObjectMappingItem> ExportProperties
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
            foreach (ObjectMappingItem MappingItem in _exportProperties)
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
                var Videos = new List<Video>();
                MMDatabase.SelectAllVideos(Videos);
                List<string> Props = new List<string>();
                foreach(ObjectMappingItem MappingItem in ExportProperties)
                {
                    if(MappingItem.Selected)
                    {
                        Props.Add(MappingItem.ObjectProperty);
                    }
                }
                Excel.Objects2Excel(Videos, Props, ExportFilePath, "videos");
                Process.Start("explorer.exe", "/select, " + ExportFilePath);//select file in explorer
            }
            else
            {
                MessageBox.Show("U hebt geen kolommen geselecteerd om te exporteren.");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void Browse()
        {

            //FolderBrowserDialog Odd = new FolderBrowserDialog { SelectedPath = Path };
            SaveFileDialog SaveFileDialog = new SaveFileDialog
                                     {
                                         InitialDirectory = Path.GetTempPath(),
                                         FileName = "exportedVideos.xls",
                                         Filter = "Excel Files(*.xls;*.xlsx)|*.XLS;*.XLSX|All files (*.*)|*.*"
                                     };
            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportFilePath = SaveFileDialog.FileName;
            }
        }
    }
}
