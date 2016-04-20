using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private async Task listenOnGame()
        {
            await Task.Run(() => findRunningGame());

            while (runningGame == null)
            {
                Thread.Sleep(100);
            }

            runningGame.GameClose += (sender) =>
            {
                Application.Current.Dispatcher.BeginInvoke
                (
                    DispatcherPriority.Background,
                    new Action(() => onGameClose())
                );
            };
        }

        private void findRunningGame()
        {
            var childProcesses = new List<Process>();

            do
            {
                childProcesses = getChildProcesses(origin.OriginProcess);
                Thread.Sleep(200);
            } while (childProcesses.Count() != 1);

            runningGame = new Game(childProcesses.First());
        }

        private List<Process> getChildProcesses(Process process)
        {
            var children = new List<Process>();
            var managementObjectSearcher = new ManagementObjectSearcher(String.Format("Select * From Win32_Process Where ParentProcessID={0}", process.Id));

            foreach (ManagementObject managementObject in managementObjectSearcher.Get())
            {
                try
                {
                    var childProcess = Process.GetProcessById(Convert.ToInt32(managementObject["ProcessID"]));
                    var installFolderName = System.IO.Path.GetFileName(selectedGame.InstallPath);

                    if (childProcess.MainModule.FileName.Contains(installFolderName))
                    {
                        children.Add(Process.GetProcessById(Convert.ToInt32(managementObject["ProcessID"])));
                    }
                }
                catch { }   //Some games do weird things that generate errors when first inspected in this loop (i.e. re-launching themselves, invalidating the old process id)
            }

            return children;
        }
    }
}
