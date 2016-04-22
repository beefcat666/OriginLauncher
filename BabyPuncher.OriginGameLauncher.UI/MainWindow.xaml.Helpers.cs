using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using BabyPuncher.OriginGameLauncher.ManagedOrigin;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class MainWindow
    {
        private void launchGame()
        {
            if (settings.Game != null)
            {
                origin.CommandLineOptions = "/StartClientMinimized origin://LaunchGame/" + settings.GameId;
                origin.StartOrigin();
            }

            if (!String.IsNullOrEmpty(settings.GameProcessExe))
            {
                waitForGame(settings.GameProcessExe);
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private async void waitForGame(string processExeName)
        {
            await Task.Run(() => getGameProcessFromExeName(processExeName));

            while (runningGame == null)
            {
                Thread.Sleep(200);
            }

            runningGame.GameClose += (sender) =>
            {
                Application.Current.Dispatcher.BeginInvoke
                (
                    DispatcherPriority.Background,
                    new Action(() => onGameClosed())
                );
            };
        }

        private void getGameProcessFromExeName(string exeName)
        {
            List<Process> process = null;  

            do
            {
                Thread.Sleep(200);
                try
                {
                    process = getChildProcesses(origin.OriginProcess)
                        .Where(x => string.Equals(Path.GetFileName(x.MainModule.FileName), exeName, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                catch { }
            } while (process.Count() != 1); //Some games will have multiple processes initially. Wait until there is only one.

            runningGame = new Game(process.First());
        }

        private List<Process> getChildProcesses(Process process)
        {
            var children = new List<Process>();
            var managementObjectSearcher = new ManagementObjectSearcher
                (String.Format("Select * From Win32_Process Where ParentProcessID={0}", process.Id));

            foreach (ManagementObject managementObject in managementObjectSearcher.Get())
            {
                try
                {
                    var childProcess = Process.GetProcessById(Convert.ToInt32(managementObject["ProcessID"]));
                    children.Add(childProcess);
                }
                catch { }   //Sometimes origin runs a few processes with short lifespans that can trigger 
                            //exceptions when we try to access them after they die
            }

            return children;
        }
    }
}
