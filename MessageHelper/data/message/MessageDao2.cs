using System;
using System.Collections.Generic;
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
//                    command.CommandText = "SELECT * FROM Message";
//                    var adapter = new SQLiteDataAdapter(command);
//                    new SQLiteCommandBuilder(adapter);
//                    adapter.Update(dataTable);


                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            Log.Debug("id:= " + row["id"]);
                            long id = (long) row["id"];
                            if (id == 0)
                            {
                                command.CommandText =
                                    $"insert into Message (testDone, testDoneTime, position, messageText, comment, " +
                                    $"messageNumber, projectId) values (" +
                                    $"'{row["testDone"]}', " +
                                    $"'{row["testDoneTime"]}', " +
                                    $"'{row["position"]}', " +
                                    $"'{row["messageText"]}', " +
                                    $"'{row["comment"]}', " +
                                    $"'{row["messageNumber"]}', " +
                                    $"'{row["projectId"]}'" +
                                    $");";
                            }
                            else
                            {
                                command.CommandText =
                                    $"update Message set " +
                                    $"testDone = '{row["testDone"]}', " +
                                    $"testDoneTime = '{row["testDoneTime"]}', " +
                                    $"position = '{row["position"]}', " +
                                    $"messageText = '{row["messageText"]}', " +
                                    $"comment = '{row["comment"]}', " +
                                    $"messageNumber = '{row["messageNumber"]}', " +
                                    $"projectId = '{row["projectId"]}' " +
                                    $"where id = {row["id"]};";
                            }

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }


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