using BabyPuncher.OriginGameLauncher.UI.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class MainWindow
    {
        private void launchButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedGame != null)
            {
                settings.GameId = selectedGame.Id;
                settings.Game = selectedGame.Name;
                origin.CommandLineOptions = "/StartClientMinimized origin://LaunchGame/" + selectedGame.Id;
                origin.StartOrigin();
                launchButton.IsEnabled = false;
                detectedGamesComboBox.IsEnabled = false;
            }

            SettingsModel.SaveSettings(settings);

            if (!String.IsNullOrEmpty(settings.GameProcessExe))
            {
                waitForGame(settings.GameProcessExe);
            }
        }

        private void windowClosed(object sender, EventArgs e)
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

        private void onGameClose()
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
    }
}
