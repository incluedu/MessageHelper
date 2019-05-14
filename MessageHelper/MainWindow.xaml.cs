using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows;
using log4net;
using MessageHelper.data.message;

namespace MessageHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /*
         * CONSTANTS
         */
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /*
         * PROPERTIES
         */
        public DataTable MessageDataTable { get; private set; }
        private List<Message> MessageList { get; set; } = new List<Message>();

        /*
         * VARIABLES
         */
        private DataSet messageDataSet;

        /*
         * CONSTRUCTORS
         */
        public MainWindow()
        {
            Log.Info("Startup");

            InitializeComponent();

            messageDataSet = MessageDao2.GetMessages();
            MessageDataTable = messageDataSet.Tables[0];
            MessagesDataGrid.ItemsSource = MessageDataTable.DefaultView;
        }

        /*
         * PRIVATE METHODS
         */
        private void ImportTiaPortalMessages_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ImportS7ClassicMessage_OnClick(object sender, RoutedEventArgs e)
        {
            var importS7ClassicMessages = new ImportS7ClassicMessages
            {
                Messages = MessageList,
                MessageDataTable = MessageDataTable
            };
            var result = importS7ClassicMessages.Start();

            if (result)
            {
                MessagesDataGrid.ItemsSource = null;
                // MessagesDataGrid.ItemsSource = MessageList;
                MessagesDataGrid.ItemsSource = MessageDataTable.DefaultView;
            }

            Status.Text = importS7ClassicMessages.StatusText;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            MessageDao2.UpdateMessages(messageDataSet);
            //MessageDao.UpdateMessages(MessageList);
        }
    }
}