using Azure_BackUpper.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Azure_BackUpper
{
    class BackUpper
    {
        public static string path = @"c:\temp\MyBackup.txt";// Bkp's File Path

        static void Main(string[] args)
        {
            Connection con = new Connection();

            try
            {
                Console.WriteLine("Connecting to the Database");
                var connection = con.Connect();
                var command = connection.CreateCommand();

                command.CommandText = @"
                    SELECT T.name AS Table_Name ,
                           C.name AS Column_Name ,
                           P.name AS Data_Type ,
                           P.max_length AS Size
                    FROM   sys.objects AS T
                           JOIN sys.columns AS C ON T.object_id = C.object_id
                           JOIN sys.types AS P ON C.system_type_id = P.system_type_id
                    WHERE  T.type_desc = 'USER_TABLE';";

                Console.WriteLine("Retriving Data");

                SqlDataReader dtreader = command.ExecuteReader();

                List <Table> tables = new List<Table>();
                var tableAux = new Table();
                string lastTableName = "";
                while (dtreader.Read())
                {
                    if (!String.IsNullOrEmpty(lastTableName))
                    {
                        if (dtreader["Table_Name"].ToString() != lastTableName)
                        {
                            tables.Add(tableAux);

                            tableAux = new Table();

                            tableAux.Table_Name = dtreader["Table_Name"].ToString();
                            tableAux.Columns.Add(new Column()
                            {
                                Column_Name = dtreader["Column_Name"].ToString(),
                                Data_Type = dtreader["data_Type"].ToString(),
                                Size = int.Parse(dtreader["Size"].ToString())
                            });

                            lastTableName = tableAux.Table_Name;
                        }
                        else
                        {
                            tableAux.Columns.Add(new Column()
                            {
                                Column_Name = dtreader["Column_Name"].ToString(),
                                Data_Type = dtreader["data_Type"].ToString(),
                                Size = int.Parse(dtreader["Size"].ToString())
                            });
                        }
                    }
                    else
                    {
                        tableAux.Table_Name = dtreader["Table_Name"].ToString();
                        tableAux.Columns.Add(new Column()
                        {
                            Column_Name = dtreader["Column_Name"].ToString(),
                            Data_Type = dtreader["data_Type"].ToString(),
                            Size = int.Parse(dtreader["Size"].ToString())
                        });

                        lastTableName = tableAux.Table_Name;
                    }
                }
                tables.Add(tableAux);

                Console.WriteLine("Closing the Database Connection");
                con.Close_connection(connection);

                //----------------------------------------------

                Console.WriteLine("Creating The Backup File");
                // Create the Backup Text File
                if (!File.Exists(path))
                {
                    // Create the file.
                    using (FileStream fs = File.Create(path))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes("your backup is created");
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                }

                Console.WriteLine("Preparing data");
                
                foreach (var table in tables)
                {
                    WriteInFile(table.ToString());
                }
            }
            catch
            {
                throw;
            }
        }

        public static void WriteInFile(string text)
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(text + "\n");
            }
        }
    }
}
