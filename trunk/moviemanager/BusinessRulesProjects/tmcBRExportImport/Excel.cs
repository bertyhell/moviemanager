using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Tmc.BusinessRules.ExportImport
{
    public class Excel
    {

        public static List<string> GetWorkSheets(string path)
        {
            List<string> Names = new List<string>();
            FileStream File = null;
            try
            {
                File = new FileStream(path, FileMode.Open, FileAccess.Read);
                HSSFWorkbook Workbook = new HSSFWorkbook(File);

                for (int I = 0; I < Workbook.NumberOfSheets; I++)
                {
                    Names.Add(Workbook.GetSheetName(I));
                }

            }
            finally
            {
                if(File != null) File.Close();
            }
            return Names;
        }

        public static List<string> GetHeaders(string path, int worksheetIndex)
        {
            List<string> Headers = new List<string>();
            FileStream File = null;
            try
            {

                File = new FileStream(path, FileMode.Open, FileAccess.Read);
                HSSFWorkbook Workbook = new HSSFWorkbook(File);
                ISheet Worksheet = Workbook.GetSheetAt(worksheetIndex);

                //get first row
                IRow Row = Worksheet.GetRow(0);
                //        //TODO 010 remove empty rows and columns before import

                for (int ColIndex = 0; ColIndex < Row.LastCellNum; ColIndex++){
                    Headers.Add(Row.Cells[ColIndex].StringCellValue);
                }
            }
            finally
            {
                if (File != null) File.Close();
            }
            return Headers;
        }

        public static void Objects2Excel(IList<Video> objects, IList<String> props, string filepath, string sheetName)
        {

            if (props != null && props.Count != 0)
            {


                HSSFWorkbook Workbook = new HSSFWorkbook();

                ISheet Sheet = Workbook.CreateSheet(sheetName);

                //write headers
                IRow Row = Sheet.CreateRow(0);
                for (int ColIndex = 0; ColIndex < props.Count; ColIndex++)
                {
                    ICell Cell = Row.CreateCell(ColIndex);
                    Cell.SetCellType(CellType.STRING);
                    Cell.SetCellValue(props[ColIndex]);
                }
                //write values
                for (int RowIndex = 1; RowIndex <= objects.Count; RowIndex++)
                {
                    Row = Sheet.CreateRow(RowIndex);
                    for (int ColIndex = 0; ColIndex < props.Count; ColIndex++)
                    {
                        ICell Cell = Row.CreateCell(ColIndex);
                        Cell.SetCellType(CellType.STRING);
                        object Value = objects[RowIndex - 1].GetType().GetProperty(props[ColIndex]).GetValue(objects[RowIndex - 1], null);
                        if (Value != null)
                        {
                            Cell.SetCellValue(Value.ToString());
                        }
                    }
                }
                FileStream File = null;
                try
                {
                    File = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                    Workbook.Write(File);
                }
                finally
                {
                    if (File != null) File.Close();
                }
            }
        }

        //TODO 080 add automatic split up between video and movie and serie --> if property requested that video doesn't support --> take movie or serie
        public static List<Video> Excel2Videos(string filePath, int worksheetIndex, List<ExcelMappingItem> mappings)
        {
            List<Video> Data = new List<Video>();
            FileStream File = null;

            try
            {
                IList<String> Headers = GetHeaders(filePath, worksheetIndex);//TODO 030 make override method that gets row object instead of opening and reading twice

                //create hashtable for mappings
                IDictionary<int, string> HeaderIndex2PropertyMappings = new Dictionary<int, string>();
                foreach (var ExcelMappingItem in mappings)
                {
                    int ColumnNumber = Headers.IndexOf(ExcelMappingItem.ExcelColumn);
                    if( ColumnNumber >= 0 )
                    {
                        HeaderIndex2PropertyMappings.Add(new KeyValuePair<int, string>(ColumnNumber, ExcelMappingItem.MMProperty));
                    }else
                    {
                        throw new DataException("Excel Column name in mappingitem doesn't exist in excel file. Excel Column: " + ExcelMappingItem.ExcelColumn + " --> MMProperty: " + ExcelMappingItem.MMProperty + ".");
                    }
                }

                //read data from excel
                File = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                HSSFWorkbook Workbook = new HSSFWorkbook(File);
                ISheet Worksheet = Workbook.GetSheetAt(worksheetIndex);

                IEnumerator Rows = Worksheet.GetRowEnumerator();

                while(Rows.MoveNext())//skip headers
                {
                    Video Video = new Video();

                    IRow Row = (IRow) Rows.Current;

                    //for every column that needs to be imported
                    foreach (var ColumnIndex in HeaderIndex2PropertyMappings.Keys)
                    {
                        //if cell is not null
                        if(! String.IsNullOrEmpty(Row.Cells[ColumnIndex].StringCellValue))
                        {
                            //set corresponding property dynamically in video object
                            Video.GetType().GetProperty(HeaderIndex2PropertyMappings[ColumnIndex]).SetValue(Video, Row.Cells[ColumnIndex].StringCellValue, null);
                        }
                    }
                    //add filled in Video to list of data objects
                    Data.Add(Video);
                }
            }
            finally
            {
                if (File != null) File.Close();
            }

            return Data;
        }
    }
}
