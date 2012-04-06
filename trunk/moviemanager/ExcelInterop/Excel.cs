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
            List<string> Worksheets = new List<string>();
            Application ExcelApp = null;

            try
            {
                ExcelApp = new Application();

                Workbook Wb = ExcelApp.Workbooks.Open(path);
                Worksheets.AddRange(from Worksheet Ws in Wb.Worksheets select Ws.Name);
            }
            finally
            {
                if ((ExcelApp != null))
                {
                    ExcelApp.Quit();
                }
            }

            return Worksheets;
        }

        public static List<string> GetHeaders(string path, string worksheet)
        {
            List<string> Headers = new List<string>();
            Application ExcelApp = null;

            try
            {
                ExcelApp = new Application();

                Workbook Wb = ExcelApp.Workbooks.Open(path);
                Worksheet Ws = (Worksheet) Wb.Worksheets.Item[worksheet];

                Range Range = Ws.Range["A1"];
                //TODO 010 remove empty rows and columns before import
                while (Range.Value2 != null)
                {
                    Headers.Add(Range.Value2);
                    Range = Range.Offset[0, 1];
                }
                return Headers;
            }
                //TODO 030 seen in lessons, check how to correct rethrow
            finally
            {
                if ((ExcelApp != null))
                {
                    ExcelApp.Quit();
                }
            }
        }

        public static void Data2Excel(DbDataReader data, IList<string> headers, string worksheetName)
        {
            if (headers == null) return;
            //is easier by: ExcelLibrary.DataSetHelper.CreateWorkbook("MyExcelFile.xls", ds); 
            //direct dataset to excelfile (didn't implement to excercise
            Application ExcelApp = new Application {Visible = true};
            Workbook Wb = ExcelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet Ws = Wb.ActiveSheet;
            Ws.Name = worksheetName;

            Range Range = Ws.Range["A1"];

            int NumberOfColumns = headers.Count();
            for (int I = 0; I < NumberOfColumns; I++)
            {
                Range.Value2 = headers.ElementAt(I);
                Range.Columns.AutoFit();
                Range = Range.Offset[0, 1];
            }


            while (data.Read())
            {
                //next row
                Range = Range.Offset[1, -NumberOfColumns];
                for (int Col = 0; Col <= NumberOfColumns - 1; Col++)
                {
                    Range.Value2 = data[Col];
                    Range = Range.Offset[0, 1];
                }
            }

            //auto size columns
            Ws.UsedRange.Columns.AutoFit();
        }

        public static List<List<string>> Excel2Data(string filePath, string worksheet, List<string> headers)
        {
            Application ExcelApp = null;
            List<List<string>> Data;
            try
            {
                ExcelApp = new Application();
                Workbook Wb = ExcelApp.Workbooks.Add(filePath);
                Worksheet Ws = (Worksheet) Wb.Worksheets.Item[worksheet];

                if (Ws.UsedRange.Rows.Count < 2)
                {
                    throw new IOException("Excel bestand bevat geen datarijen");
                }

                List<string> AllHeaders = GetHeaders(filePath, worksheet);


                //collect requested columns
                List<int> ImportColumns = headers.Select(header => AllHeaders.IndexOf(header) + 1).ToList();

                Data = new List<List<string>>();
                for (int Row = 1; Row <= Ws.UsedRange.Rows.Count; Row++)
                {
                    List<string> ObjectValues = ImportColumns.Select(col => ((Range) Ws.UsedRange.Item[Row, col]).Value2).Cast<string>().ToList();
                    Data.Add(ObjectValues);
                }
                Wb.Close();
            }
            finally//TODO 010 error bericht
            {
                if ((ExcelApp != null))
                {
                    ExcelApp.Quit();
                }
            }
            return Data;
        }
    }
}
