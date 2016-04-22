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
using System.Windows.Threading;
using BabyPuncher.OriginGameLauncher.UI.Properties;
using BabyPuncher.OriginGameLauncher.ManagedOrigin;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class MainWindow : Window
    {
        private Origin origin;
        private IList<OriginGame> detectedOriginGames;
        private OriginGame selectedGame;
        private Game runningGame;
        private Settings settings;

        public MainWindow()
        {
            InitializeComponent();

            settings = Settings.GetSettings();
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
