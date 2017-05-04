using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace WBSummationCore
{
    class ExcelWriter
    {
        private string connectionString;
        OleDbConnection MyConnection;

        public ExcelWriter(string path)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            // XLSX - Excel 2007, 2010, 2012, 2013Microsoft.ACE.OLEDB.14.0
            props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
            props["Extended Properties"] = "Excel 12.0 XML";
            props["Data Source"] = path;

            // XLS - Excel 2003 and Older
            //props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
            //props["Extended Properties"] = "Excel 8.0";
            //props["Data Source"] = "C:\\MyExcel.xls";

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            connectionString = sb.ToString();
            MyConnection = new OleDbConnection(connectionString);
            MyConnection.Open();
        }
        public void insertData(string code, string name, decimal cost)
        {
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = MyConnection;
            myCommand.CommandText = string.Format("Insert into [Лист1$] (WBS, NAME, COST) values(\'{0}\',\'{1}\',\'{2}\')", code, name, Convert.ToString(cost));
            myCommand.ExecuteNonQuery();
        }
    }
}
