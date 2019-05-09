using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace MessageHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImportTiaPortalMessages_OnClick(object sender, RoutedEventArgs e)
        {

            ImportTiaPortalMessages importTiaPortalMessages = new ImportTiaPortalMessages();
            Boolean result = importTiaPortalMessages.Start();

            if (result)
            {
                MessageBox.Show("Import");
            }
            else
            {
                MessageBox.Show("Cancel!");
            }



/*
            DataTable sheet1 = new DataTable();
            OleDbConnectionStringBuilder csbuilder = new OleDbConnectionStringBuilder();
            csbuilder.Provider = "Microsoft.ACE.OLEDB.12.0";
            csbuilder.DataSource = @"C:\c#\MessageHelper\LSR-Fehlertexte_FT2_CHA1200904.xlsx";
            csbuilder.Add("Extended Properties", "Excel 12.0 Xml;HDR=YES");
            
            using (OleDbConnection connection = new OleDbConnection(csbuilder.ConnectionString))
            {
                connection.Open();
                string selectSql = @"SELECT * FROM [Sheet1$]";
                MessageBox.Show(selectSql);
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(selectSql, connection))
                {
                    adapter.Fill(sheet1);
                    
                    
                    //dataGridView1.DataSource = sheet1;
                }
                connection.Close();
            }
*/
        }

        private void ImportS7ClassicMessage_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
