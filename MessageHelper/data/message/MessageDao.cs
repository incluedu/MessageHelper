using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;

namespace MessageHelper.data.message
{
    public class MessageDao
    {
        public static List<Message> GetMessages()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<Message>("select * from Message", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void UpdateMessages(List<Message> messages)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                //todo
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}