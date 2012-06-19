using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.IO;
//using ExcelLibrary.SpreadSheet;
//using Microsoft.Office.Interop.Excel;
using ExcelLibrary.SpreadSheet;
using Model;
using QiHe.CodeLib;

//using QiHe.CodeLib;
//using Workbook = Microsoft.Office.Interop.Excel.Workbook;
//using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace ExcelInterop
{
    public class Excel
    {

        public static List<string> GetWorkSheets(string path)
        {


            // open xls file
            Workbook Book = Workbook.Load(path);

            List<string> Names = new List<string>();
            for (int I = 0; I < Book.Worksheets.Count; I++)
            {
                Names.Add(Book.Worksheets[0].Name);
            }
            return Names;

            //    List<string> Worksheets = new List<string>();
            //    Application ExcelApp = null;

            //    try
            //    {
            //        ExcelApp = new Application();

            //        Workbook Wb = ExcelApp.Workbooks.Open(path);
            //        Worksheets.AddRange(from Worksheet Ws in Wb.Worksheets select Ws.Name);
            //    }
            //    finally
            //    {
            //        if ((ExcelApp != null))
            //        {
            //            ExcelApp.Quit();
            //        }
            //    }

            //    return Worksheets;
        }

        public static List<string> GetHeaders(string path, int worksheetIndex)
        {
            Workbook Book = Workbook.Load(path);
            Worksheet Sheet = Book.Worksheets[worksheetIndex];
            List<string> Headers = new List<string>();

            int RowIndex = Sheet.Cells.FirstRowIndex;
            for (int ColIndex = 0; ColIndex < Sheet.Cells.LastColIndex; ColIndex++)
            {
                Headers.Add(Sheet.Cells[RowIndex, ColIndex].Value.ToString());
            }

            return Headers;

            //// traverse cells
            //foreach (Pair<Pair<int, int>, Cell> Cell in Sheet.Cells)
            //{

            //    dgvCells[Cell.Left.Right, Cell.Left.Left].Value = Cell.Right.Value;
            //}

            //// traverse rows by Index
            //for (int rowIndex = Sheet.Cells.FirstRowIndex;
            //     rowIndex <= Sheet.Cells.LastRowIndex;
            //     rowIndex++)
            //{
            //    Row row = Sheet.Cells.GetRow(rowIndex);
            //    for (int colIndex = row.FirstColIndex;
            //         colIndex <= row.LastColIndex;
            //         colIndex++)
            //    {
            //        Cell cell = row.GetCell(colIndex);
            //    }
            //}



            //    List<string> Headers = new List<string>();
            //    Application ExcelApp = null;

            //    try
            //    {
            //        ExcelApp = new Application();

            //        Workbook Wb = ExcelApp.Workbooks.Open(path);
            //        Worksheet Ws = (Worksheet) Wb.Worksheets.Item[worksheet];

            //        Range Range = Ws.Range["A1"];
            //        //TODO 010 remove empty rows and columns before import
            //        while (Range.Value2 != null)
            //        {
            //            Headers.Add(Range.Value2);
            //            Range = Range.Offset[0, 1];
            //        }
            //        return Headers;
            //    }
            //        //TODO 030 seen in lessons, check how to correct rethrow
            //    finally
            //    {
            //        if ((ExcelApp != null))
            //        {
            //            ExcelApp.Quit();
            //        }
            //    }
        }

        public static void Videos2Excel(List<Video> objects, IList<String> props, string filepath, string worksheetname)
        {
            if (props != null && props.Count != 0)
            {
                Workbook Workbook = new Workbook();
                Worksheet Worksheet = new Worksheet(worksheetname);
                Worksheet.Cells[0, 1] = new Cell((short)1);
                Worksheet.Cells[2, 0] = new Cell(9999999);
                Worksheet.Cells[3, 3] = new Cell((decimal)3.45);
                Worksheet.Cells[2, 2] = new Cell("Text string");
                Worksheet.Cells[2, 4] = new Cell("Second string");
                Worksheet.Cells[4, 0] = new Cell(32764.5, "#,##0.00");
                Worksheet.Cells[5, 1] = new Cell(DateTime.Now, @"YYYY\-MM\-DD");
                Worksheet.Cells.ColumnWidth[0, 1] = 3000;
                Workbook.Worksheets.Add(Worksheet);
                Workbook.Save(filepath);

                //// open xls file
                //Workbook book = Workbook.Load(file);
                //Worksheet sheet = book.Worksheets[0];

                //// traverse cells
                //foreach (Pair<Pair<int, int>, Cell> cell in sheet.Cells)
                //{
                //    dgvCells[cell.Left.Right, cell.Left.Left].Value = cell.Right.Value;
                //}

                //// traverse rows by Index
                //for (int rowIndex = sheet.Cells.FirstRowIndex;
                //     rowIndex <= sheet.Cells.LastRowIndex;
                //     rowIndex++)
                //{
                //    Row row = sheet.Cells.GetRow(rowIndex);
                //    for (int colIndex = row.FirstColIndex;
                //         colIndex <= row.LastColIndex;
                //         colIndex++)
                //    {
                //        Cell cell = row.GetCell(colIndex);
                //    }
                //}
            }
            //create new xls file
        }

        public static List<List<string>> Excel2Data(string filePath, int worksheetIndex, List<string> headers)
        {
            return new List<List<string>>();
        }

        //public static void Data2Excel(DbDataReader data, IList<string> headers, string worksheetName)
        //{
        //    if (headers == null) return;
        //    //is easier by: ExcelLibrary.DataSetHelper.CreateWorkbook("MyExcelFile.xls", ds); 
        //    //direct dataset to excelfile (didn't implement to excercise
        //    Application ExcelApp = new Application {Visible = true};
        //    Workbook Wb = ExcelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        //    Worksheet Ws = Wb.ActiveSheet;
        //    Ws.Name = worksheetName;

        //    Range Range = Ws.Range["A1"];

        //    int NumberOfColumns = headers.Count();
        //    for (int I = 0; I < NumberOfColumns; I++)
        //    {
        //        Range.Value2 = headers.ElementAt(I);
        //        Range.Columns.AutoFit();
        //        Range = Range.Offset[0, 1];
        //    }


        //    while (data.Read())
        //    {
        //        //next row
        //        Range = Range.Offset[1, -NumberOfColumns];
        //        for (int Col = 0; Col <= NumberOfColumns - 1; Col++)
        //        {
        //            Range.Value2 = data[Col];
        //            Range = Range.Offset[0, 1];
        //        }
        //    }

        //    //auto size columns
        //    Ws.UsedRange.Columns.AutoFit();
        //}

        //public static List<List<string>> Excel2Data(string filePath, string worksheet, List<string> headers)
        //{
        //    Application ExcelApp = null;
        //    List<List<string>> Data;
        //    try
        //    {
        //        ExcelApp = new Application();
        //        Workbook Wb = ExcelApp.Workbooks.Add(filePath);
        //        Worksheet Ws = (Worksheet) Wb.Worksheets.Item[worksheet];

        //        if (Ws.UsedRange.Rows.Count < 2)
        //        {
        //            throw new IOException("Excel bestand bevat geen datarijen");
        //        }

        //        List<string> AllHeaders = GetHeaders(filePath, worksheet);


        //        //collect requested columns
        //        List<int> ImportColumns = headers.Select(header => AllHeaders.IndexOf(header) + 1).ToList();

        //        Data = new List<List<string>>();
        //        for (int Row = 1; Row <= Ws.UsedRange.Rows.Count; Row++)
        //        {
        //            List<string> ObjectValues = ImportColumns.Select(col => ((Range) Ws.UsedRange.Item[Row, col]).Value2).Cast<string>().ToList();
        //            Data.Add(ObjectValues);
        //        }
        //        Wb.Close();
        //    }
        //    finally//TODO 010 error bericht
        //    {
        //        if ((ExcelApp != null))
        //        {
        //            ExcelApp.Quit();
        //        }
        //    }
        //    return Data;
        //}
    }
}
