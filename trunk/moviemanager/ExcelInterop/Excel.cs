using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace ExcelInterop
{
    public class Excel
    {

        public static List<string> GetWorkSheets(string path)
        {
            List<string> worksheets = new List<string>();
            Application excelApp = null;

            try
            {
                excelApp = new Application();

                Workbook wb = excelApp.Workbooks.Open(path);
                worksheets.AddRange(from Worksheet ws in wb.Worksheets select ws.Name);
            }
            finally
            {
                if ((excelApp != null))
                {
                    excelApp.Quit();
                }
            }

            return worksheets;
        }

        public static List<string> GetHeaders(string path, string worksheet)
        {
            List<string> headers = new List<string>();
            Application excelApp = null;

            try
            {
                excelApp = new Application();

                Workbook wb = excelApp.Workbooks.Open(path);
                Worksheet ws = (Worksheet) wb.Worksheets.Item[worksheet];

                Range range = ws.Range["A1"];
                //TODO 010 remove empty rows and columns before import
                while (range.Value2 != null)
                {
                    headers.Add(range.Value2);
                    range = range.Offset[0, 1];
                }
                return headers;
            }
                //TODO 030 seen in lessons, check how to correct rethrow
            finally
            {
                if ((excelApp != null))
                {
                    excelApp.Quit();
                }
            }
        }

        public static void Data2Excel(DbDataReader data, IEnumerable<string> headers, string worksheetName)
        {
            if (headers == null) return;
            //is easier by: ExcelLibrary.DataSetHelper.CreateWorkbook("MyExcelFile.xls", ds); 
            //direct dataset to excelfile (didn't implement to excercise
            Application excelApp = new Application {Visible = true};
            Workbook wb = excelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = wb.ActiveSheet;
            ws.Name = worksheetName;

            Range range = ws.Range["A1"];
            foreach (string header in headers)
            {
                range.Value2 = header;
                range.Columns.AutoFit();
                range = range.Offset[0, 1];
            }

            dynamic numberOfColumns = headers.Count();

            while (data.Read())
            {
                //next row
                range = range.Offset[1, -numberOfColumns];
                for (int col = 0; col <= numberOfColumns - 1; col++)
                {
                    range.Value2 = data[col];
                    range = range.Offset[0, 1];
                }
            }

            //auto size columns
            ws.UsedRange.Columns.AutoFit();
        }

        public static List<List<string>> Excel2Data(string filePath, string worksheet, List<string> headers)
        {
            Application excelApp = null;
            List<List<string>> data;
            try
            {
                excelApp = new Application();
                Workbook wb = excelApp.Workbooks.Add(filePath);
                Worksheet ws = (Worksheet) wb.Worksheets.Item[worksheet];

                if (ws.UsedRange.Rows.Count < 2)
                {
                    throw new IOException("Excel bestand bevat geen datarijen");
                }

                List<string> allHeaders = GetHeaders(filePath, worksheet);


                //collect requested columns
                List<int> importColumns = headers.Select(header => allHeaders.IndexOf(header) + 1).ToList();

                data = new List<List<string>>();
                for (int row = 1; row <= ws.UsedRange.Rows.Count; row++)
                {
                    List<string> objectValues = importColumns.Select(col => ((Range) ws.UsedRange.Item[row, col]).Value2).Cast<string>().ToList();
                    data.Add(objectValues);
                }
                wb.Close();
            }
            catch
            {
                throw;
                //TODO 010 error bericht
            }
            finally
            {
                if ((excelApp != null))
                {
                    excelApp.Quit();
                }
            }
            return data;
        }
    }
}
