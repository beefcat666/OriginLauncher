using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class MainWindow
    {
        private void onTestButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedGame = detectedOriginGames
                .FirstOrDefault(x => x.Name == (string)detectedGamesComboBox.SelectedValue);

            settings.Game = selectedGame.Name;
            settings.GameId = selectedGame.Id;
            settings.GameId = selectedGame.Id;
            settings.GameProcessExe = gameExeTextBox.Text;
            settings.Save();

            var runner = new ProcessStartInfo()
            {
                FileName = "OGLRunner.exe"
            };

            using (Process process = Process.Start(runner))
            {
                Hide();
                process.WaitForExit();
                Show();
            }

        }

        private void onSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedGame = detectedOriginGames
                .FirstOrDefault(x => x.Name == (string)detectedGamesComboBox.SelectedValue);

            settings.Game = selectedGame.Name;
            settings.GameId = selectedGame.Id;
            settings.GameId = selectedGame.Id;
            settings.GameProcessExe = gameExeTextBox.Text;
            settings.Save();
        }

        private void onGameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gameExeTextBox.Text = string.Empty;
            testButton.IsEnabled = true;
        }

        private void onBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Executable Files (*.exe)|*.exe"
            };

            openFileDialog.ShowDialog();

            var result = Path.GetFileName(openFileDialog.FileName);
            gameExeTextBox.Text = (!string.IsNullOrWhiteSpace(result)) ? result : gameExeTextBox.Text;
        }
    }
}
