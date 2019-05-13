using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace MessageHelper.data.message
{
    public class MessageDao2
    {
        /*
         * PUBLIC STATIC METHODS
         */
        public static DataSet GetMessages(int projectId = 0)
        {
            var dataSet = new DataSet();

            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                new SQLiteDataAdapter("select * from Message", connection).Fill(dataSet);
                connection.Close();
            }
            return dataSet;
        }

        public static void UpdateMessages(DataSet dataSet)
        {
            
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