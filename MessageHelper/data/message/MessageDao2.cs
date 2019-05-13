using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Windows;
using log4net;

namespace MessageHelper.data.message
{
    public class MessageDao2
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /*
         * PUBLIC STATIC METHODS
         */
        public static DataSet GetMessages(int projectId = 0)
        {
            var dataSet = new DataSet();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
                {
                    connection.Open();
                    new SQLiteDataAdapter("select * from Message", connection).Fill(dataSet);
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            return dataSet;
        }

        public static void UpdateMessages(DataSet dataSet)
        {
            if (dataSet == null) return;


            try
            {
                using (var connection = new SQLiteConnection(LoadConnectionString()))
                {
                    connection.Open();

                    var dataTable = dataSet.Tables[0];
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Message";
                    var adapter = new SQLiteDataAdapter(command);
                    new SQLiteCommandBuilder(adapter);
                    adapter.Update(dataTable);

                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message);
                Log.Debug(exception.StackTrace);
                MessageBox.Show(exception.Message);
            }
        }


        /*
         * PRIVATE METHODS
         */
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}