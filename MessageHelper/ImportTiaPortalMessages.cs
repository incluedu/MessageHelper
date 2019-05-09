using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using ExcelDataReader;
using Microsoft.Win32;

namespace MessageHelper
{
    public class ImportTiaPortalMessages
    {
        private DataSet ds;


        public ImportTiaPortalMessages()
        {
        }

        public Boolean Start()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|Old Excel files (*.xls)|*.xls";
            if (fileDialog.ShowDialog() == true)
            {
                Import(fileDialog.FileName);
                return true;
            }

            return false;
        }

        private void Import(String filename)
        {
            MessageBox.Show(Path.GetExtension(filename));

            var extension = Path.GetExtension(filename).ToLower();
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var sw = new Stopwatch();
                sw.Start();
                IExcelDataReader reader = null;
                if (extension == ".xls")
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (extension == ".xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else if (extension == ".csv")
                {
                    reader = ExcelReaderFactory.CreateCsvReader(stream);
                }

                if (reader == null)
                    return;

                var openTiming = sw.ElapsedMilliseconds;
                // reader.IsFirstRowAsColumnNames = firstRowNamesCheckBox.Checked;
                using (reader)
                {
                    ds = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        UseColumnDataType = false,
                        ConfigureDataTable = tableReader => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    });
                }


                MessageBox.Show("Elapsed: " + sw.ElapsedMilliseconds + " ms (" + openTiming + " ms to open)");

                var tablenames = GetTablenames(ds.Tables);
                //sheetCombo.DataSource = tablenames;

                if (tablenames.Count > 0)
                {
                    //sheetCombo.SelectedIndex = 0;
                }

                // dataGridView1.DataSource = ds;
                // dataGridView1.DataMember
            }
        }

        private static IList<string> GetTablenames(DataTableCollection tables)
        {
            var tableList = new List<string>();
            foreach (var table in tables)
            {
                tableList.Add(table.ToString());
            }

            return tableList;
        }
    }
}