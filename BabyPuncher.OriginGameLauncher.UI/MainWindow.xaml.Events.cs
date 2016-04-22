using BabyPuncher.OriginGameLauncher.UI.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class MainWindow
    {
        private void onLaunchButtonClicked(object sender, RoutedEventArgs e)
        {
            if (selectedGame != null)
            {
                settings.GameId = selectedGame.Id;
                settings.Game = selectedGame.Name;
                origin.CommandLineOptions = "/StartClientMinimized origin://LaunchGame/" + selectedGame.Id;
                origin.StartOrigin();
                launchButton.IsEnabled = false;
                detectedGamesComboBox.IsEnabled = false;

                origin.OriginUnexpectedClose += () =>
                {
                    Application.Current.Dispatcher.BeginInvoke
                        (
                            DispatcherPriority.Background,
                            new Action(() => onOriginClosed())
                        );
                };
            }

            settings.Save();

            if (!String.IsNullOrEmpty(settings.GameProcessExe))
            {
                waitForGame(settings.GameProcessExe);
            }
        }

        private void onWindowClosed(object sender, EventArgs e)
        {
            origin.KillOrigin();
        }

        private void onGameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedGame = detectedOriginGames
                .Where(x => x.Name == (string)detectedGamesComboBox.SelectedValue)
                .FirstOrDefault();
            launchButton.IsEnabled = true;
        }

        private void onGameClosed()
        {
            if (settings.Silent)
            {
                Application.Current.Shutdown();
            }
            else
            {
                launchButton.IsEnabled = true;
                detectedGamesComboBox.IsEnabled = true;
            }
        }

        private void onOriginClosed()
        {
            Application.Current.Shutdown();
        }
    }
}
