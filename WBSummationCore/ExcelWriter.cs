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
        // добавить финализатор

        private OleDbConnection session;

        public ExcelWriter(string path)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            // XLSX - Excel 2007, 2010, 2012, 2013 Microsoft.ACE.OLEDB.14.0
            props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
            props["Extended Properties"] = "Excel 12.0 XML";
            props["Data Source"] = path;

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            session = new OleDbConnection(sb.ToString());
            session.Open();                                                                                                                                                                 // Может кидать исключение если в системе нет Microsoft.ACE.OLEDB.12.0
        }
        public void insertData(string code, string name, decimal cost)                                                                                                                      // Переписать интерфейс подобно readColumns
        {
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = session;
            myCommand.CommandText = string.Format("Insert into [Лист1$] (WBS, NAME, COST) values(\'{0}\',\'{1}\',\'{2}\')", code, name, Convert.ToString(cost));                             // Изменить если загружаем не в Лист1
            myCommand.ExecuteNonQuery();
        }
    }
}
