using System.Windows;
using log4net;
using log4net.Config;

namespace MessageHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        protected override void OnStartup(StartupEventArgs e)
        {
            //XmlConfigurator.Configure();
            log.Info("=============  Messagehelper start  =============");
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            log.Info("=============  Messagehelper close  =============");
            base.OnExit(e);
        }
    }
}