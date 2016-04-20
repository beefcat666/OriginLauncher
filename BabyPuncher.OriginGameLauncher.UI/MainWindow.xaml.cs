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
                .Where(x => x.Name == Settings.Default.Game)
                .Select(x => x.Name)
                .FirstOrDefault();

            if (!String.IsNullOrEmpty(lastPlayedGame))
            {
                detectedGamesComboBox.SelectedItem = lastPlayedGame;
            }
        }
    }
}
