using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using BabyPuncher.OriginGameLauncher.UI.Properties;
using BabyPuncher.OriginGameLauncher.ManagedOrigin;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class MainWindow : Window
    {
        private Origin origin;
        private IList<DetectedOriginGame> detectedOriginGames;
        private DetectedOriginGame selectedGame;
        private Game runningGame;

        public MainWindow()
        {
            InitializeComponent();
            LaunchButton.IsEnabled = false;
            origin = new Origin();

            detectedOriginGames = Origin.DetectOriginGames();

            if (detectedOriginGames == null)
            {
                return;
            }

            DataContext = new ViewModel(detectedOriginGames.Select(x => x.Name).ToList());
            
            var lastPlayedGame = detectedOriginGames
                .Where(x => x.Name == Settings.Default.LastPlayedGame)
                .Select(x => x.Name)
                .FirstOrDefault();

            if (!String.IsNullOrEmpty(lastPlayedGame))
            {
                detectedGamesComboBox.SelectedItem = lastPlayedGame;
            }
        }
        
        private async Task listenOnGame()
        {
            await Task.Run(() => findRunningGame());
        }

        private void findRunningGame()
        {
            var childProcesses = new List<Process>();

            do
            {
                childProcesses = GetChildProcesses(origin.OriginProcess);
                Thread.Sleep(200);
            } while (childProcesses.Count() != 1);

            runningGame = new Game(childProcesses.First());
        }

        private List<Process> GetChildProcesses(Process process)
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
        
#region Event Handling
        private void LaunchButton_Click(object _sender, RoutedEventArgs e)
        {
            if (selectedGame != null)
            {
                origin.CommandLineOptions = "/StartClientMinimized origin://LaunchGame/" + selectedGame.Id;
                origin.StartOrigin();
                LaunchButton.IsEnabled = false;
                detectedGamesComboBox.IsEnabled = false;
            }

            listenOnGame();

            while (runningGame == null)
            {
                Thread.Sleep(100);
            }

            runningGame.GameClose += (sender) =>
            {
                OnGameClose();
            };
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            origin.KillOrigin();
        }

        private void OnGameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedGame = detectedOriginGames
                .Where(x => x.Name == (string)detectedGamesComboBox.SelectedValue)
                .FirstOrDefault();
            LaunchButton.IsEnabled = true;
        }

        private void OnGameClose()
        {
            LaunchButton.IsEnabled = true;
            detectedGamesComboBox.IsEnabled = true;
        }
#endregion
    }

    public class ViewModel
    {
        public IList<string> DetectedOriginGameNames { get; set; }

        public ViewModel(IList<string> detectedOriginGameNames)
        {
            DetectedOriginGameNames = detectedOriginGameNames;
        }
    }
}
