using System;
using System.IO;
using System.Data;
using System.Text;

namespace ModbusDevicesSimulator
{
    class CSVReader
    {
        static public bool readCSV(string filePath, out DataTable dt)
        {
            dt = new DataTable();

            try
            {
                System.Text.Encoding encoding = Encoding.Default;

                FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);

                System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);
        
                string[] aryLine = null;
                string[] tableHead = null;
                string strLine;

                int columnCount = 0;
               
                bool IsFirst = true;
                
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (IsFirst == true)
                    {
                        tableHead = strLine.Split(',');
                        IsFirst = false;
                        columnCount = tableHead.Length;
                        
                        for (int i = 0; i < columnCount; i++)
                        {
                            DataColumn dc = new DataColumn(tableHead[i]);
                            dt.Columns.Add(dc);
                        }
                    }
                    else
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (aryLine != null && aryLine.Length > 0)
                {
                    dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                }

                sr.Close();
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while reading CSV: {e.Message}");
                return false;
            }
        }
    }
}
