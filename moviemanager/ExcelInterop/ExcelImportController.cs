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
            _excelColumns = new List<string>();
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
                    _mappingItems = new ObservableCollection<ExcelMappingItem>();
                    _mappingItems.Add(new ExcelMappingItem
                    {
                        MMColumn = "Id*",
                        ExcelColumn = GetBestMatch("Id", ExcelColumns)
                    });
                    _mappingItems.Add(new ExcelMappingItem
                    {
                        MMColumn = "Categorie (auto=leeg)",
                        ExcelColumn = GetBestMatch("Categorie", ExcelColumns)
                    });
                    _mappingItems.Add(new ExcelMappingItem
                    {
                        MMColumn = "Ratio (auto = 1)",
                        ExcelColumn = GetBestMatch("Ratio", ExcelColumns)
                    });
                    _mappingItems.Add(new ExcelMappingItem
                    {
                        MMColumn = "Stock totaal aantal (auto = 0)",
                        ExcelColumn = GetBestMatch("Stock totaal aantal", ExcelColumns)
                    });
                    _mappingItems.Add(new ExcelMappingItem
                    {
                        MMColumn = "Stock rest aantal (auto = 0)",
                        ExcelColumn = GetBestMatch("Stock rest aantal", ExcelColumns)
                    });

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

        private List<string> _excelColumns;
        public List<string> ExcelColumns
        {
            get { return _excelColumns; }
            set { _excelColumns = value; }
        }

        public void GetImportFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "Producten.xlsx";
            openFileDialog.Filter = "Excel Bestanden(*.xls;*.xlsx)|*.XLS;*.XLSX|All files (*.*)|*.*";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                FilePath = openFileDialog.FileName;
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
                    List<string> excelHeaders = (from mappingItem in MappingItems where mappingItem.ExcelColumn != "Auto" select mappingItem.ExcelColumn).ToList();
                    List<List<string>> data = Excel.Excel2Data(FilePath, SelectedWorksheet, excelHeaders);

                    string errorMessage = "";

                    //create video objects
                    int idIndex = -1;
                    int categoryIndex = -1;
                    int ratioIndex = -1;
                    int stockAantalIndex = -1;
                    int stockRestAantalIndex = -1;

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

                    Video video = new Video();
                    for (int i = 1; i <= data.Count - 1; i++)
                    {
                        try
                        {
                            video.Id = int.Parse(data[i][idIndex]);
                            video.IdImdb = (categoryIndex == -1 ? "" : data[i][categoryIndex]);
                            //video.FPRatio = (categoryIndex == -1 ? 1 : data[i][ratioIndex]);
                            //video.FPAantal = (categoryIndex == -1 ? 0 : data[i][stockAantalIndex]);
                            //video.FPRestAantal = (categoryIndex == -1 ? 0 : data[i][stockRestAantalIndex]);
                            //video.FPFuifId = _fuifId;

                            MMDatabase.InsertVideoHDD(video);
                            //TODO 020 ask to update items when first duplicate is encountered
                        }
                        catch (FormatException e)
                        {
                            errorMessage += "\n" + "ProductId is geen getal: '" + data[i][idIndex] + "' (overgeslagen)\n\t" + e.Message;
                        }
                        catch (SqlException e)
                        {
                            errorMessage += "\n" + "Probleem met toevoegen tot database: '" + string.Join("|", data[i]) + "' (overgeslagen)\n\t" + e.Message;
                        }
                    }


                    if (string.IsNullOrWhiteSpace(errorMessage))
                    {
                        //TODO 060 add error messages
                        //Microsoft.Windows.Controls.MessageBox.Show("Importeren voltooid\n Aantal geimporteerde producten: " + aantalImportedProducts, "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    else
                    {
                        //TODO 060 add error messages
                        //Microsoft.Windows.Controls.MessageBox.Show("Importeren voltooid met enkele fouten: " + errorMessage + "\n" + "Aantal geimporteerde producten: " + aantalImportedProducts, "Succes met enkele fouten", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }
                }
                else
                {
                    //TODO 060 add error messages
                    //Microsoft.Windows.Controls.MessageBox.Show("Bij verplichte items(*) mag de 'Auto'-waarde niet geselecteerd zijn?", "Verplichte velden", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (IOException e)
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

            int[,] matrix = new int[firstString.Length + 1, secondString.Length + 1];

            for (int i = 0; i <= firstString.Length; i++)
            {
                matrix[i, 0] = i;
            }
            // deletion
            for (int j = 0; j <= secondString.Length; j++)
            {
                matrix[0, j] = j;
            }
            // insertion
            for (int i = 0; i <= firstString.Length - 1; i++)
            {
                for (int j = 0; j <= secondString.Length - 1; j++)
                {
                    if (firstString[i] == secondString[j])
                    {
                        matrix[i + 1, j + 1] = matrix[i, j];
                    }
                    else
                    {
                        matrix[i + 1, j + 1] = Math.Min(matrix[i, j + 1] + 1, matrix[i + 1, j] + 1);
                        //deletion or insertion
                        //substitution
                        matrix[i + 1, j + 1] = Math.Min(matrix[i + 1, j + 1], matrix[i, j] + 1);
                    }
                }
            }
            return matrix[firstString.Length, secondString.Length];
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

            int longestLenght = Math.Max(firstString.Length, secondString.Length);
            int distance = GetLevensteinDistance(firstString, secondString);
            double percent = distance / Convert.ToDouble(longestLenght);
            return 1 - percent;
        }

        public static string GetBestMatch(string s1, List<string> list)
        {
            string bestMatch = list[0];
            double bestSimilarity = 0;
            foreach (string s2 in list)
            {
                double similarity = GetSimilarity(s1, s2);
                if (similarity > bestSimilarity)
                {
                    bestMatch = s2;
                    bestSimilarity = similarity;
                }
            }
            return bestMatch;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
