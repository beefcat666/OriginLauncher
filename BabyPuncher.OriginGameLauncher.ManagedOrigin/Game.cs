using System.Diagnostics;
using System.Threading.Tasks;

namespace BabyPuncher.OriginGameLauncher.ManagedOrigin
{
    public class Game
    {
        public Process GameProcess;

        public delegate void GameCloseEvent(object sender);
        public event GameCloseEvent GameClose;
        
        public bool IsGameRunning()
        {
            return (Process.GetProcessesByName(GameProcess.ProcessName).Length == 0) ? false : true;
        }

        public Game(Process gameProcess)
        {
            GameProcess = gameProcess;
            listenForClose();
        }

        private async void listenForClose()
        {
            await Task.Run(() => GameProcess.WaitForExit());
            if (GameClose != null) GameClose(this);
        }
    }

    public class OriginGame
    {
        public string Id { get; set; }
        public string InstallPath { get; set; }
        public string Name { get; set; }
    }
}