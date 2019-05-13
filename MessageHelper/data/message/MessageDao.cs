using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using Dapper;
using log4net;

namespace MessageHelper.data.message
{
    public class MessageDao
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /*
         * PUBLIC METHODS
         */
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
            Log.Debug("Update messages in database");
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var query =
                    "insert into Message(testDone, testDoneTime, position, messageText, comment, messageNumber, projectId) values (@TestDone, @TestDoneTime, @position, @messageText, @comment, @messageNumber, @projectId)";
                connection.Execute(query, messages);
                
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