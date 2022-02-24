using System.Windows;
using System.Threading;

namespace HammerTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex? mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string ApplicationName = "Hammer Tracker";
            bool createdNew;

            mutex = new Mutex(true, ApplicationName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("You already have a instance of Hammer Tracker running. \n Please close the older instance to open another.");
                Application.Current.Shutdown();
            }
            base.OnStartup(e);
        }
    }
}
