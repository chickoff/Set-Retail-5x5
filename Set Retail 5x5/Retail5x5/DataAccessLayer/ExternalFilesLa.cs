using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using Dapper;
using Set_Retail_5x5.Retail5x5.Model.Data;
using Set_Retail_5x5.Retail5x5.Model.ExternalData;
using Excel = Microsoft.Office.Interop.Excel;

namespace Set_Retail_5x5.Retail5x5.DataAccessLayer
{
    public static class ExternalFilesLa
    {
        public static List<ChargeOnByClient> OpenSourseFile(object filePath)
        {
            var excelapp = new Excel.Application {Visible = false};
            var excelappworkbook = excelapp.Workbooks.Open((string)filePath,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);
            var excelsheets = excelappworkbook.Worksheets;

            var excelworksheet = (Excel.Worksheet)excelsheets.Item[1];
            var wsName = excelworksheet.Name;
            excelapp.Quit();
           
            Marshal.FinalReleaseComObject(excelapp);
            var oConn = new System.Data.OleDb.OleDbConnection
            {
                ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0 ;Data Source=" + (string) filePath +
                                   ";Extended Properties=\"Excel 12.0;HDR=Yes;\";"
            };
           var r =  oConn.Query<ChargeOnByClient>($"select * from [{wsName}$]").ToList();
           return r;
        }


        public static void ToExcel(List<Set10ExchangeLoyClient> cli)
        {
            int x = 7, y = cli.Count, shift = 1;

            var excelResultDoc = new Excel.Application {SheetsInNewWorkbook = 1};
            excelResultDoc.Workbooks.Add(Type.Missing);
            Excel.Range R;
            Excel._Worksheet w;
            w = excelResultDoc.Worksheets[1];
            var dataExport = new object[y + shift, x];

            dataExport[0, 0] = "Id";
            dataExport[0, 1] = "Фамилия";
            dataExport[0, 2] = "Имя";
            dataExport[0, 3] = "Отчество";
            dataExport[0, 4] = "Дата рождения";
            dataExport[0, 5] = "Телефон";
            dataExport[0, 6] = "Категория";

            for (var i = 0; i < cli.Count; i++)
            {
                dataExport[i + shift, 0] = cli[i].Set10Guid;
                dataExport[i + shift, 1] = cli[i].LastName;
                dataExport[i + shift, 2] = cli[i].FirstName;
                dataExport[i + shift, 3] = cli[i].MiddleName;
                dataExport[i + shift, 4] = cli[i].BirthDate;
                dataExport[i + shift, 5] = cli[i].MobilePhone;
                dataExport[i + shift, 6] = cli[i].CategoryId;
            }

            int y1 = 1, x1 = 1, y2 = dataExport.GetLength(0), x2 = dataExport.GetLength(1);

           
            excelResultDoc.Visible = true;
            R = w.Range[w.Cells[y1, x1], w.Cells[y2,x2]];
            R.set_Value(Type.Missing, dataExport);
            R.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            R.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            R.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

        }
    }
}
