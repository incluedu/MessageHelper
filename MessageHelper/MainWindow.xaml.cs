using System.Collections.Generic;
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
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public List<Message> MessageList { get; set; } = new List<Message>();

        /*
         * VARIABLES
         */


        /*
         * CONSTRUCTORS
         */
        public MainWindow()
        {
            //log4net.Config.XmlConfigurator.Configure();
            Log.Info("Startup");

            InitializeComponent();

            MessageList = MessageDao.GetMessages();
            var messageSet = MessageDao2.GetMessages();

            //MessagesDataGrid.ItemsSource = MessageList;
            MessagesDataGrid.ItemsSource = messageSet.Tables[0].DefaultView;
        }

        /*
         * PRIVATE METHODS
         */
        private void ImportTiaPortalMessages_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void ImportS7ClassicMessage_OnClick(object sender, RoutedEventArgs e)
        {
            var importS7ClassicMessages = new ImportS7ClassicMessages {Messages = MessageList};
            var result = importS7ClassicMessages.Start();

            if (result)
            {
                MessagesDataGrid.ItemsSource = null;
                MessagesDataGrid.ItemsSource = MessageList;
            }

            Status.Text = importS7ClassicMessages.StatusText;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            MessageDao.UpdateMessages(MessageList);
        }
    }
}