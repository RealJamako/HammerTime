using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.IO;


namespace HammerTracker
{
    public partial class MainWindow : Window
    {
        #region Hammer Tracker : Easter EGG
        private int _clicks = 0;
        #endregion

        #region Hammer Tracker : Instance Of Hammer Tracker
        HammerTrackerClass hammertracker = new HammerTrackerClass();
        #endregion

        #region Hammer Tracker : Stopwatch & Timer
        private readonly Stopwatch stopwatch;
        private readonly System.Timers.Timer timer;
        #endregion

        [DllImport("Shell32.dll")]
        private static extern int ShellExecuteA(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirecotry, int nShowCmd);

        public static int ShellExecute(string filename, string parameters = "", string workingFolder = "", string verb = "open", int windowOption = 1)
        {
            IntPtr parentWindow = IntPtr.Zero;

            try
            {
                int pid = ShellExecuteA(parentWindow, verb, filename, parameters, workingFolder, windowOption);
                return pid;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 0;
            }

        }

        public MainWindow()
        {
            InitializeComponent();

            stopwatch = new Stopwatch();    
            timer = new System.Timers.Timer();
        }

        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => HammerTrackerTimer.Text = stopwatch.Elapsed.ToString(@"hh\:mm\:ss"));
            Application.Current.Dispatcher.Invoke(() => hammertracker.SessionLasted = HammerTrackerTimer.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Hammer Tracker : Setting Window Placement (Bottom-right corner)
            double offset = 15;
            var desktopWorkingArea = SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width - offset;
            this.Top = desktopWorkingArea.Bottom - this.Height - offset;
            #endregion

            #region Hammer Tracker : Does Past Session File Exist
            hammertracker.CheckForPastSessionFile();
            #endregion

            #region Hammer Tracker : Open Hammer
            OpenHammer();
            #endregion
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            #region Hammer Tracker : Timer Elapsed
            timer.Elapsed += OnTimerElapsed;
            #endregion

            #region Hammer Tracker : Ping For Hammer
            _ = hammertracker.PingHammer(stopwatch, timer);
            #endregion
        }

        private void OpenSessions_Click(object sender, RoutedEventArgs e)
        {
            ShellExecute("HammerTracker - Past Sessions.txt",Environment.CurrentDirectory);
        }

        private static void OpenHammer()
        { 
            var Directory = Environment.CurrentDirectory;
            var FileNameH = "hammer.exe";
            var FileNameHPP = "hammerplusplus.exe";
            var pathH = Path.Combine(Directory, FileNameH);
            var pathHPP = Path.Combine(Directory, FileNameHPP);
            if (File.Exists(pathHPP))
            {
                ShellExecute(FileNameHPP, Directory);
            }
            else if (!File.Exists(pathHPP))
            {
                ShellExecute(FileNameH, Directory);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            hammertracker.PrintSessionTime();
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            #region Hammer Tracker : Easter EGG
            _clicks++;
            if (_clicks == 7)
            {
                NavigateURL("https://www.youtube.com/watch?v=yhHZQEmWQs8");
            }
            #endregion
        }

        private static void NavigateURL(string webpage)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = webpage,
                UseShellExecute = true
            });
        }
    }
}
