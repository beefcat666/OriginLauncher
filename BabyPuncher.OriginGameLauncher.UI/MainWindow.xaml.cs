using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BabyPuncher.OriginGameLauncher.ManagedOrigin;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class MainWindow : Window
    {
        private Origin origin;
        private IList<OriginGame> detectedOriginGames;
        private OriginGame selectedGame;
        private Game runningGame;
        private OriginGameLauncherSettings settings;

        public MainWindow()
        {
            InitializeComponent();

            settings = new OriginGameLauncherSettings();
            origin = new Origin();

            if (settings.Silent)
            {
                IsEnabled = false;
                Hide();
                launchGame();
            }

            launchButton.IsEnabled = false;
            detectedOriginGames = Origin.DetectOriginGames();

            if (detectedOriginGames == null)
            {
                MessageBox.Show("No Origin games found on your system. Origin may not be installed properly.", "Oops",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Application.Current.Shutdown();
                return;
            }

            DataContext = new ViewModel(detectedOriginGames.Select(x => x.Name).ToList());
        }
    }
}
