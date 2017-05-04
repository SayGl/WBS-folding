using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace WBSummationCore
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemdStyle compatibilityTest = new SystemdStyle("Проверка наличия OLEDB драйвера в системе.");
            compatibilityTest.showMessage();

            if (!oledbExistTest())
            {
                compatibilityTest.failMessage();
                Console.WriteLine("Microsoft.ACE.OLEDB.12.0 не обнаружен в системе, используйте программу в соответствии с разрядностью вашей системы.");
                return;
            }
            compatibilityTest.okMessage();

            ///////////////////////////////////////////////////////////////////////////
            Console.Write("Введите названия excel файла, в котором содержится информация о структуре участка (Structure.xlsx по умолчанию): ");
            string nameStructure = Console.ReadLine();

            SystemdStyle existStructTest = new SystemdStyle("Поиск файла структуры работ на участке.");
            existStructTest.showMessage();

            if (nameStructure.Length == 0)
            {
                nameStructure = "Structure.xlsx";
            }

            if (!File.Exists(nameStructure))
            {
                existStructTest.failMessage();
                Console.WriteLine("Файл с именем {0} не найден.", nameStructure);
                return;
            }

            existStructTest.okMessage();

            //////////////////////////////////////////////////////////////////////////

            Console.Write("Введите название листа excel, на котором содержится информация о структуре работ (Лист1 по умолчанию): ");

            string structureListName = Console.ReadLine();

            if (structureListName.Length == 0)
            {
                structureListName = "Лист1";
            }

            
            //////////////////////////////////////////////////////////////////////////

            Console.Write("Введите названия excel файла, в котором содержится информация о cтоимости работ на участке (Costs.xls по умолчанию): ");
            string nameCost = Console.ReadLine();

            SystemdStyle existKeyValueTest = new SystemdStyle("Поиск файла стоимости работ на участке.");
            existStructTest.showMessage();

            if (nameCost.Length == 0)
            {
                nameCost = "Costs.xls";
            }

            if (!File.Exists(nameCost))
            {
                existStructTest.failMessage();
                Console.WriteLine("Файл с именем {0} не найден.", nameCost);
                return;
            }

            existStructTest.okMessage();
            //////////////////////////////////////////////////////////////////////////
            Console.Write("Введите название листа excel, на котором содержится информация о стоимости работ (TASK по умолчанию): ");

            string costListName = Console.ReadLine();

            if (costListName.Length == 0)
            {
                costListName = "TASK";
            }
            //////////////////////////////////////////////////////////////////////////


            ExcelReader readStructure = new ExcelReader(nameStructure);
            ExcelReader readData = new ExcelReader(nameCost);


            FoldableTree tree = new FoldableTree(FoldableTree.listToStructure(readStructure.readColumns(0, structureListName , new List<string> { "Код WBS" }))
                                                ,FoldableTree.listToData(readData.readColumns(1, costListName, new List<String> { @"wbs_id", @"task_name", @"target_drtn_hr_cnt" })));


            ExcelWriter writer = new ExcelWriter("Output.xlsx");


            Dictionary<String, decimal> cost = tree.printTree();

            List<List<String>> names = readStructure.readColumns(0, structureListName, new List<string> { "Название WBS" });

            int i = 0;

            foreach (var record in cost)
            {
                writer.insertData(record.Key, names[i][0], record.Value);
                i++;
            }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
        }

        static public bool oledbExistTest()
        {
            var reader = OleDbEnumerator.GetRootEnumerator();

            var list = new List<String>();
            while (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i) == "SOURCES_NAME")
                    {
                        list.Add(reader.GetValue(i).ToString());
                        if(reader.GetValue(i).ToString() == "Microsoft.ACE.OLEDB.12.0") return true;
                    }
                }
                //Console.WriteLine("{0} = {1}", reader.GetName(0), reader.GetValue(0));
            }
            reader.Close();
            return false;
        }

    }
}
