using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class HammerTrackerClass
{
    private readonly string filename = "HammerTracker - Past Sessions.txt";
    public string SessionLasted = string.Empty;
    private static bool hasOpened;

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
            if (HammerOpen())
            {
                sw.Start();
                tm.Start();
                hasOpened = true;
            }
            else if (!HammerOpen())
            {
                sw.Stop();
                sw.Reset();
                if (hasOpened)
                {
                    PrintSessionTime();
                    hasOpened = !hasOpened;
                }
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

        File.AppendAllText(Path.Combine(Environment.CurrentDirectory, filename), content + Environment.NewLine);
    }
}
