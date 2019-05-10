using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using ExcelDataReader;
using log4net;
using MessageHelper.data.message;
using Microsoft.Win32;

namespace MessageHelper
{
    /// <summary>
    /// Import Messages from S7 classic LTW 'LSR Fehlertexte' file
    /// </summary>
    public class ImportS7ClassicMessages
    {
        /*
         * CONSTANTS
         */
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /*
         * PROPERTIES
         */
        public string StatusText { get; private set; }
        public int CounterUpdate { get; private set; }
        public int CounterAdd { get; private set; }
        public List<Message> Messages { get; set; } = new List<Message>();


        /*
         * VARIABLES
         */
        private DataSet _ds;
        //private List<Message> _messages = new List<Message>();


        /*
         * PUBLIC METHODS
         */
        public bool Start()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|Old Excel files (*.xls)|*.xls";
            if (fileDialog.ShowDialog() == true)
            {
                return Import(fileDialog.FileName);
            }

            return false;
        }


        /*
         * PRIVATE METHODS
         */

        private bool Import(string path)
        {
            Log.Debug("Start --> Import messages from " + path);

            var extension = Path.GetExtension(path).ToLower();
            var sw = new Stopwatch();

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
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
                    return false;

                _ds = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                });
                
                stream.Close();
            }

            var dt = _ds.Tables[0];

            var errorMessagesSheetNr = _ds.Tables.IndexOf("Fehlertexte");

            if (errorMessagesSheetNr == -1)
            {
                UpdateStatusText("Wrong file, dose not contain sheet 'Fehlertexte'!");
                return false;
            }

            foreach (DataRow row in dt.Rows)
            {
                var position = row[0] as string;
                //var visiPos = row[1] as string;
                // var prio = row[2] as string;
                var messageNumber = row[3] as string;
                // var bit = row[4] as string;
                var messageText = row[5] as string;


                if (!string.IsNullOrEmpty(position))
                {
                    UpdateMessage(new Message
                    {
                        Position = position,
                        MessageText = messageText,
                        MessageNumber = messageNumber
                    });
                }
            }

            var totalMessages = CounterUpdate + CounterAdd;
            var filename = Path.GetFileName(path);

            StatusText = "Import " + totalMessages + " messages (New:" + CounterAdd + " , Updates: " + CounterUpdate +
                         ") from " + filename + " in " + sw.ElapsedMilliseconds + "ms";
            Log.Info(StatusText);
            return true;
        }

        /// <summary>
        /// Try to update the given message in the messages list. If it is a new message it will be added to the
        /// messages list.
        /// </summary>
        /// <param name="message">Message to add</param>
        private void UpdateMessage(Message message)
        {
            foreach (var msg in Messages)
            {
                if (message.MessageNumber.Equals(msg.MessageNumber))
                {
                    msg.Position = message.Position;
                    msg.MessageText = message.MessageText;
                    CounterUpdate++;
                    return;
                }
            }

            CounterAdd++;
            Messages.Add(message);
        }


        /// <summary>
        /// Help function to update the status text, make a corresponding entry in the log file and display a message
        /// box.
        /// </summary>
        /// <param name="statusText">Text with the status message to set</param>
        private void UpdateStatusText(string statusText)
        {
            StatusText = statusText;
            Log.Info(statusText);
            MessageBox.Show(statusText, "Error!");
        }
    }
}