using System.Linq;
using System.Windows.Forms;
using Model;
using SQLite;

namespace ExcelInterop
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Data.SqlClient;

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
                PropChanged("Worksheets");
            }
        }

        private string _selectedWorksheet;
        public string SelectedWorksheet
        {
            get { return _selectedWorksheet; }
            set
            {
                _selectedWorksheet = value;
                //update mapping
                ExcelColumns = Excel.GetHeaders(FilePath, _selectedWorksheet);
                ExcelColumns.Insert(0, "Default");
                _mappingItems.Clear();
                if (ExcelColumns.Count() != 0)
                {
                    _mappingItems = new ObservableCollection<ExcelMappingItem>
                                        {
                                            new ExcelMappingItem
                                                {
                                                    MMColumn = "Id*",
                                                    ExcelColumn = GetBestMatch("Id", ExcelColumns)
                                                },
                                            new ExcelMappingItem
                                                {
                                                    MMColumn = "Categorie (auto=leeg)",
                                                    ExcelColumn = GetBestMatch("Categorie", ExcelColumns)
                                                },
                                            new ExcelMappingItem
                                                {
                                                    MMColumn = "Ratio (auto = 1)",
                                                    ExcelColumn = GetBestMatch("Ratio", ExcelColumns)
                                                },
                                            new ExcelMappingItem
                                                {
                                                    MMColumn = "Stock totaal aantal (auto = 0)",
                                                    ExcelColumn = GetBestMatch("Stock totaal aantal", ExcelColumns)
                                                },
                                            new ExcelMappingItem
                                                {
                                                    MMColumn = "Stock rest aantal (auto = 0)",
                                                    ExcelColumn = GetBestMatch("Stock rest aantal", ExcelColumns)
                                                }
                                        };
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

        public bool Import()
        {
            try
            {
                //import selected excel file
                //required fields not mapped to "default"
                if (MappingItems.All(item => !item.MMColumn.EndsWith("*") || item.ExcelColumn != "Auto"))
                {
                    List<string> ExcelHeaders = (from MappingItem in MappingItems where MappingItem.ExcelColumn != "Auto" select MappingItem.ExcelColumn).ToList();
                    List<List<string>> Data = Excel.Excel2Data(FilePath, SelectedWorksheet, ExcelHeaders);

                    string ErrorMessage = "";

                    //create video objects
                    const int idIndex = -1;
                    const int categoryIndex = -1;
                    int RatioIndex = -1;
                    int StockAantalIndex = -1;
                    int StockRestAantalIndex = -1;

                    //foreach (ExcelMappingItem item in MappingItems)
                    //{
                    //    switch (item.MMColumn)
                    //    {
                    //        case _mappingItems[0].MMColumn:
                    //            idIndex = excelHeaders.IndexOf(item.ExcelColumn);
                    //            break;
                    //        case _mappingItems[1].MMColumn:
                    //            categoryIndex = excelHeaders.IndexOf(item.ExcelColumn);
                    //            break;
                    //        case _mappingItems[2].MMColumn:
                    //            ratioIndex = excelHeaders.IndexOf(item.ExcelColumn);
                    //            break;
                    //        case _mappingItems[3].MMColumn:
                    //            stockAantalIndex = excelHeaders.IndexOf(item.ExcelColumn);
                    //            break;
                    //        case _mappingItems[4].MMColumn:
                    //            stockRestAantalIndex = excelHeaders.IndexOf(item.ExcelColumn);
                    //            break;
                    //    }
                    //}

                    Video Video = new Video();
                    for (int i = 1; i <= Data.Count - 1; i++)
                    {
                        try
                        {
                            Video.Id = int.Parse(Data[i][idIndex]);
                            Video.IdImdb = (categoryIndex == -1 ? "" : Data[i][categoryIndex]);
                            //video.FPRatio = (categoryIndex == -1 ? 1 : data[i][ratioIndex]);
                            //video.FPAantal = (categoryIndex == -1 ? 0 : data[i][stockAantalIndex]);
                            //video.FPRestAantal = (categoryIndex == -1 ? 0 : data[i][stockRestAantalIndex]);
                            //video.FPFuifId = _fuifId;

                            //TODO 020 ask to update items when first duplicate is encountered
                        }
                        catch (FormatException E)
                        {
                            ErrorMessage += "\n" + "ProductId is geen getal: '" + Data[i][idIndex] + "' (overgeslagen)\n\t" + E.Message;
                        }
                        catch (SqlException E)
                        {
                            ErrorMessage += "\n" + "Probleem met toevoegen tot database: '" + string.Join("|", Data[i]) + "' (overgeslagen)\n\t" + E.Message;
                        }
                    }


                    if (string.IsNullOrWhiteSpace(ErrorMessage))
                    {
                        //TODO 060 add error messages
                        //Microsoft.Windows.Controls.MessageBox.Show("Importeren voltooid\n Aantal geimporteerde producten: " + aantalImportedProducts, "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    //TODO 060 add error messages
                    //Microsoft.Windows.Controls.MessageBox.Show("Importeren voltooid met enkele fouten: " + errorMessage + "\n" + "Aantal geimporteerde producten: " + aantalImportedProducts, "Succes met enkele fouten", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                //TODO 060 add error messages
                //Microsoft.Windows.Controls.MessageBox.Show("Bij verplichte items(*) mag de 'Auto'-waarde niet geselecteerd zijn?", "Verplichte velden", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (IOException)
            {
                //TODO 060 add error messages
                //Microsoft.Windows.Controls.MessageBox.Show(e.Message, "Gegevens niet gevonden", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        private static int GetLevensteinDistance(string firstString, string secondString)
        {
            if (firstString == null)
            {
                throw new ArgumentNullException("firstString");
            }
            if (secondString == null)
            {
                throw new ArgumentNullException("secondString");
            }

            if (firstString == secondString)
            {
                return 0;
            }

            int[,] Matrix = new int[firstString.Length + 1, secondString.Length + 1];

            for (int i = 0; i <= firstString.Length; i++)
            {
                Matrix[i, 0] = i;
            }
            // deletion
            for (int j = 0; j <= secondString.Length; j++)
            {
                Matrix[0, j] = j;
            }
            // insertion
            for (int i = 0; i <= firstString.Length - 1; i++)
            {
                for (int j = 0; j <= secondString.Length - 1; j++)
                {
                    if (firstString[i] == secondString[j])
                    {
                        Matrix[i + 1, j + 1] = Matrix[i, j];
                    }
                    else
                    {
                        Matrix[i + 1, j + 1] = Math.Min(Matrix[i, j + 1] + 1, Matrix[i + 1, j] + 1);
                        //deletion or insertion
                        //substitution
                        Matrix[i + 1, j + 1] = Math.Min(Matrix[i + 1, j + 1], Matrix[i, j] + 1);
                    }
                }
            }
            return Matrix[firstString.Length, secondString.Length];
        }

        public static double GetSimilarity(string firstString, string secondString)
        {
            if (firstString == null)
            {
                throw new ArgumentNullException("firstString");
            }
            if (secondString == null)
            {
                throw new ArgumentNullException("secondString");
            }

            if (firstString == secondString)
            {
                return 1;
            }

            int LongestLenght = Math.Max(firstString.Length, secondString.Length);
            int Distance = GetLevensteinDistance(firstString, secondString);
            double Percent = Distance / Convert.ToDouble(LongestLenght);
            return 1 - Percent;
        }

        public static string GetBestMatch(string s1, List<string> list)
        {
            string BestMatch = list[0];
            double BestSimilarity = 0;
            foreach (string S2 in list)
            {
                double Similarity = GetSimilarity(s1, S2);
                if (Similarity > BestSimilarity)
                {
                    BestMatch = S2;
                    BestSimilarity = Similarity;
                }
            }
            return BestMatch;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
