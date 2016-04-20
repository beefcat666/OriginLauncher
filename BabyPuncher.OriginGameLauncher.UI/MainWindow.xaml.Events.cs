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
        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedGame != null)
            {
                origin.CommandLineOptions = "/StartClientMinimized origin://LaunchGame/" + selectedGame.Id;
                origin.StartOrigin();
                LaunchButton.IsEnabled = false;
                detectedGamesComboBox.IsEnabled = false;
            }

            SettingsManager.SaveUserSetting("BabyPuncher.OriginGameLauncher.UI.Properties.Settings", "Game", selectedGame.Name);
            Settings.Default.Save();

            listenOnGame();
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
            LaunchButton.IsEnabled = true;
        }

        private void onGameClose()
        {
            LaunchButton.IsEnabled = true;
            detectedGamesComboBox.IsEnabled = true;
        }
    }
}
