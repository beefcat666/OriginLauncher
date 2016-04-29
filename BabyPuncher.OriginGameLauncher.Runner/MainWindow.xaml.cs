using System;
using System.Windows;
using BabyPuncher.OriginGameLauncher.ManagedOrigin;

namespace BabyPuncher.OriginGameLauncher.Runner
{
    public partial class MainWindow : Window
    {
        private Origin origin;
        private OriginGame selectedGame;
        private Game runningGame;
        private OriginGameLauncherSettings settings;

        public MainWindow()
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "OGLRunner.exe.config");
            InitializeComponent();
            Hide();

            settings = new OriginGameLauncherSettings();

            if (string.IsNullOrEmpty(settings.GameId))
            {
                MessageBox.Show("No game found in your config file. Run OGLConfigurator.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                launchGame();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
