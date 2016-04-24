using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BabyPuncher.OriginGameLauncher.Runner
{
    public partial class MainWindow
    {
        private void onWindowClosed(object sender, EventArgs e)
        {
            origin.KillOrigin();
        }
        
        private void onGameClosed()
        {
            Application.Current.Shutdown();
        }

        private void onOriginClosed()
        {
            Application.Current.Shutdown();
        }
    }
}
