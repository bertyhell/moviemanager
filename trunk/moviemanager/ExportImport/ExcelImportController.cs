using System.Linq;
using System.Windows.Forms;
using ExcelInterop;
using Model;
using MovieManager.Common;
using MovieManager.LOG;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ExportImport
{
    public class ExcelImportController : INotifyPropertyChanged
    {
        public ExcelImportController()
        {
            ExcelColumns = new List<string>();
            _mappingItems = new ObservableCollection<ExcelMappingItem>();
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                Worksheets = Excel.GetWorkSheets(_filePath);
            }
        }

        private List<string> _worksheets;
        public List<string> Worksheets
        {
            get { return _worksheets; }
            set
            {
                _worksheets = value;
                if(Worksheets.Count > 0) SelectedWorksheetIndex = 0; //select first item explicitly --> updates mapping items
                PropChanged("Worksheets");
            }
        }

        private int _selectedWorksheetIndex;
        public int SelectedWorksheetIndex
        {
            get { return _selectedWorksheetIndex; }
            set
            {
                _selectedWorksheetIndex = value;
                //update mapping
                ExcelColumns = Excel.GetHeaders(FilePath, _selectedWorksheetIndex);
                ExcelColumns.Insert(0, "Default");
                _mappingItems.Clear();
                if (ExcelColumns.Count() != 0)
                {
                    _mappingItems = new ObservableCollection<ExcelMappingItem>();
                    foreach (var ExportableProperty in Video.ExportableProperties)
                    {
                        _mappingItems.Add(new ExcelMappingItem
                            {
                                MMProperty = ExportableProperty,
                                ExcelColumn = StringSimilarity.GetBestMatch(ExportableProperty, ExcelColumns)
                            });
                    }
                                        //{
                                        //    new ExcelMappingItem
                                        //        {
                                        //            MMProperty = "Id*",
                                        //            ExcelColumn = StringSimilarity.GetBestMatch("Id", ExcelColumns)
                                        //        },
                                        //    new ExcelMappingItem
                                        //        {
                                        //            MMProperty = "Categorie (auto=leeg)",
                                        //            ExcelColumn = StringSimilarity.GetBestMatch("Categorie", ExcelColumns)
                                        //        },
                                        //    new ExcelMappingItem
                                        //        {
                                        //            MMProperty = "Ratio (auto = 1)",
                                        //            ExcelColumn = StringSimilarity.GetBestMatch("Ratio", ExcelColumns)
                                        //        },
                                        //    new ExcelMappingItem
                                        //        {
                                        //            MMProperty = "Stock totaal aantal (auto = 0)",
                                        //            ExcelColumn = StringSimilarity.GetBestMatch("Stock totaal aantal", ExcelColumns)
                                        //        },
                                        //    new ExcelMappingItem
                                        //        {
                                        //            MMProperty = "Stock rest aantal (auto = 0)",
                                        //            ExcelColumn = StringSimilarity.GetBestMatch("Stock rest aantal", ExcelColumns)
                                        //        }
                                        //};
                }
                PropChanged("MappingItems");
            }
        }

        private ObservableCollection<ExcelMappingItem> _mappingItems;
        public ObservableCollection<ExcelMappingItem> MappingItems
        {
            get { return _mappingItems; }
            set { _mappingItems = value; }
        }

        public List<string> ExcelColumns { get; set; }

        public void GetImportFile()
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog
                                                {
                                                    FileName = "Producten.xlsx",
                                                    Filter =
                                                        "Excel Bestanden(*.xls;*.xlsx)|*.XLS;*.XLSX|All files (*.*)|*.*"
                                                };
            DialogResult Result = OpenFileDialog.ShowDialog();
            if (Result == DialogResult.OK) // Test result.
            {
                FilePath = OpenFileDialog.FileName;
                PropChanged("FilePath");
            }
        }


        public void PropChanged(string arg)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(arg));
            }
        }

        public void Import()
        {
            try
            {
                //import selected excel file
                //required fields not mapped to "default"
                if (MappingItems.All(item => !item.MMProperty.EndsWith("*") || item.ExcelColumn != "Auto"))
                {
                    List<ExcelMappingItem> ImportMappingItems = new List<ExcelMappingItem>();
                    foreach (var MappingItem in MappingItems)
                    {
                        if (MappingItem.ExcelColumn != "Auto")
                        {
                            ImportMappingItems.Add(MappingItem);
                        }
                    }
                    List<Video> Data = Excel.Excel2Videos(FilePath, SelectedWorksheetIndex, ImportMappingItems);
                    MMDatabase.InsertVideosHDD(Data);
                }
            }catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GlobalLogger.Instance.MovieManagerLogger.Error(GlobalLogger.FormatExceptionForLog(typeof(ExcelImportController).FullName, "Import", Ex.Message));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
