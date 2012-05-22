using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace eSocium.Web.Models.OpenQuestions.Tables
{
    public interface IWorksheet
    {
        string this[int row, int column] { get; }
    }

    public static class IWorksheetExtension 
    {
        public static int RowCount(this IWorksheet worksheet) {
            int answer = 0;
            while(worksheet[answer, 0] != null) 
            {
                ++answer;
            }
            return answer;
        }
        public static int ColumnCount(this IWorksheet worksheet) {
            int answer = 0;
            while(worksheet[0, answer] != null) 
            {
                ++answer;
            }
            return answer;
        }
        public static string[] Header(this IWorksheet worksheet) {
            string[] result = new string[worksheet.ColumnCount() - 1];
            for (int curr_column = 1; curr_column < worksheet.ColumnCount(); ++curr_column)
            {
                result[curr_column - 1] = worksheet[0, curr_column];
            }
            return result;
        }
    }

    public class WorksheetXlsx : IWorksheet 
    {
        ExcelRange cells;
        public WorksheetXlsx(ExcelPackage package, int sheet_number) 
        {
            if (package.Workbook.Worksheets.Count < sheet_number && sheet_number > 0)
            {
                throw new Exception("Sheet number is not valid");
            }
            cells = package.Workbook.Worksheets[sheet_number].Cells;
        }
        public string this[int row, int column] 
        {
            get 
            {
                return (cells[row + 1, column + 1].Value ?? null).ToString();
            }
        }
    }

    public class WorksheetXls : IWorksheet
    {
        ISheet workbook;
        public WorksheetXls(HSSFWorkbook package, int sheet_number) 
        {
            --sheet_number;
            if (package.Workbook.NumSheets <= sheet_number && sheet_number >= 0)
            {
                throw new Exception("Sheet number is not valid");
            }
            workbook = package.GetSheetAt(sheet_number);
        }
        public string this[int row, int column]
        {
            get
            {
                try
                {
                    ICell cell = workbook.GetRow(row).Cells[column];
                    return cell.ToString();
                }
                catch
                {
                    return null;
                }
            }
        }
    }

    class Methods 
    {
        static public IWorksheet GetWorksheetFromHttpFile(HttpPostedFileBase xlsFile, int sheet_num) 
        {
            if (xlsFile == null)
            {
                throw new Exception("No file uploaded");
            }
            if (xlsFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new WorksheetXlsx(new ExcelPackage(xlsFile.InputStream), sheet_num);
            }
            if (xlsFile.ContentType == "application/vnd.ms-excel")
            {
                return new WorksheetXls(new HSSFWorkbook(xlsFile.InputStream), sheet_num);
            }
            throw new Exception("Wrong file type");
        }
    }
}
