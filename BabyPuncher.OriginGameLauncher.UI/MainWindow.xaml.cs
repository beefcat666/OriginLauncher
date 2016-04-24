using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using BabyPuncher.OriginGameLauncher.ManagedOrigin;
using BabyPuncher.OriginGameLauncher.Runner;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class MainWindow : Window
    {
        private IList<OriginGame> detectedOriginGames;
        private OriginGame selectedGame;
        private OriginGameLauncherSettings settings;
        private static readonly string configFileName = "OGLRunner.exe.config";

        public MainWindow()
        {
            if (!File.Exists(configFileName))
            {
                createBlankConfigFile();
            }

            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configFileName);
            InitializeComponent();

            settings = new OriginGameLauncherSettings();
            testButton.IsEnabled = false;
            detectedOriginGames = Origin.DetectOriginGames();



            if (detectedOriginGames == null)
            {
                MessageBox.Show("No Origin games found on your system. Origin may not be installed properly.", "Oops",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Application.Current.Shutdown();
                return;
            }

            DataContext = new ViewModel()
            {
                DetectedOriginGameNames = detectedOriginGames.Select(x => x.Name).ToList(),
                GameExeFileName = settings.GameProcessExe,
                Game = settings.Game
            };
        }
    }
}
