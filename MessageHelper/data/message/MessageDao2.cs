using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace MessageHelper.data.message
{
    public class MessageDao2
    {
        public static List<Message> GetMessages(int projectId = 0)
        {
            List<Message> messages = new List<Message>();
            
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                var sql = "select * from Message where projectId=" + projectId;
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    messages.Add(new Message()
                    {
                        Id = (int) reader["Id"],
                        
                    });
                }

                SQLiteDataAdapter DB = new SQLiteDataAdapter(sql, connection);
                DataSet DS = new DataSet();
                DB.Fill(DS);

                connection.Close();
            }
            
        }
        
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

    }