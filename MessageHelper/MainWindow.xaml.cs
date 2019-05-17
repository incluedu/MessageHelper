using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            MessagesDataGrid.Columns.Add(new DataGridCheckBoxColumn());
            
            MessagesDataGrid.Columns[0].Visibility = Visibility.Hidden;
            MessagesDataGrid.Columns[1].Visibility = Visibility.Visible;
            MessagesDataGrid.Columns[2].Visibility = Visibility.Hidden;
            MessagesDataGrid.Columns[3].Visibility = Visibility.Visible;
            MessagesDataGrid.Columns[4].Visibility = Visibility.Visible;
            MessagesDataGrid.Columns[5].Visibility = Visibility.Visible;
            MessagesDataGrid.Columns[6].Visibility = Visibility.Visible;
            MessagesDataGrid.Columns[7].Visibility = Visibility.Hidden;
            MessagesDataGrid.Columns[8].Visibility = Visibility.Visible;
            
            MessagesDataGrid.Columns[5].Width = 220;
//            MessagesDataGrid.Columns[1].DisplayIndex = 4;
//            MessagesDataGrid.Columns[6].DisplayIndex = 1;
//            MessagesDataGrid.Columns[8].DisplayIndex = 0;

            MessagesDataGrid.Columns[0].IsReadOnly = true;
            MessagesDataGrid.Columns[1].IsReadOnly = false;
            MessagesDataGrid.Columns[2].IsReadOnly = true;
            MessagesDataGrid.Columns[3].IsReadOnly = true;
            MessagesDataGrid.Columns[4].IsReadOnly = true;
            MessagesDataGrid.Columns[5].IsReadOnly = false;
            MessagesDataGrid.Columns[6].IsReadOnly = true;
            MessagesDataGrid.Columns[7].IsReadOnly = true;
            MessagesDataGrid.Columns[8].IsReadOnly = true;

            MessagesDataGrid.Columns[0].Header = "ID";
            MessagesDataGrid.Columns[1].Header = "Test done";
            MessagesDataGrid.Columns[2].Header = "Test done time";
            MessagesDataGrid.Columns[3].Header = "Position";
            MessagesDataGrid.Columns[4].Header = "Message";
            MessagesDataGrid.Columns[5].Header = "Comment";
            MessagesDataGrid.Columns[6].Header = "Number";
            MessagesDataGrid.Columns[7].Header = "Project ID";
            MessagesDataGrid.Columns[8].Header = "Alarm";
            

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