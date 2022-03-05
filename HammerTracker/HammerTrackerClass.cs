using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class HammerTrackerClass
{
    private readonly string filename = "HammerTracker - Past Sessions.txt";
    public string SessionLasted = string.Empty;
    private bool isRunning = false;

    private static bool HammerOpen()
    {
        foreach (Process process in Process.GetProcesses())
        {
            if (process.ProcessName == "hammer" || process.ProcessName == "hammerplusplus")
            {
                return true;
            }
        }
        return false;
    }

    public async Task PingHammer(Stopwatch sw, System.Timers.Timer tm)
    {
        while (true)
        {
            var delaytask = Task.Delay(10000);
            if (HammerOpen() == true)
            {
                if (!isRunning)
                {
                    isRunning = true;
                    sw.Start();
                    tm.Start();
                }
            }
            else if (HammerOpen() == false)
            {
                if (isRunning)
                {
                    sw.Stop();
                    sw.Reset();
                    PrintSessionTime();
                    isRunning = false;
                }
                //if hammer is closed
                sw.Stop();
            }
            await delaytask;
        }
    }

    public void CheckForPastSessionFile()
    {
        var dir = Environment.CurrentDirectory;
        var FilePath = $"{dir}/{filename}";

        if (File.Exists(FilePath) == true)
        {
            return;
        }
        else
        {
            CreateSessionFile();
        }
    }

    private static void CreateSessionFile()
    {
        string[] Title = { "Below are the past sessions you have had in Hammer : " };
        string DocPath = Environment.CurrentDirectory;

        using (StreamWriter output = new StreamWriter(Path.Combine(DocPath, "HammerTracker - Past Sessions.txt")))
        {
            foreach (string title in Title)
            { 
                output.WriteLine(title);
                output.Close();
            }
        }
    }

    public void PrintSessionTime()
    {
        string timestamp = DateTime.Now.ToString();
        string content = $"{timestamp} : You spent {SessionLasted} working on your project!";

        if (HammerOpen() == true)
            File.AppendAllText(Path.Combine(Environment.CurrentDirectory, filename), content + Environment.NewLine);
        else
            return;
    }
}
