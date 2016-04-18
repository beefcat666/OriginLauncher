using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BabyPuncher.OriginGameLauncher.ManagedOrigin
{
    public class Origin
    {
        public string OriginPath;
        public string CommandLineOptions;
        public bool RestartOrigin;
        public bool ClosedSafely;

        public Process OriginProcess;

        public delegate void OriginCloseEvent(object sender, OriginCloseEventArgs e);
        public event OriginCloseEvent OriginClose;

        public delegate void OriginUnexpectedCloseEvent(object sender);
        public event OriginUnexpectedCloseEvent OriginUnexpectedClose;

        public Game Game;

        private static readonly string OriginLocalCacheDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Origin\\LocalContent";

        public Origin(string commandLineOptions)
        {
            CommandLineOptions = commandLineOptions;
            OriginClose += Origin_OriginClose;
        }

        public Origin() : this("/StartClientMinimized") { } //Minimized Origin by default

        #region Origin
        private void Origin_OriginClose(object sender, OriginCloseEventArgs args)
        {
            if (args.RestartOrigin) CreateUnmanagedInstance();
        }

        public void StartOrigin()
        {
            OriginPath = GetOriginPath();
            if (OriginPath == null) return;
            var originProcessInfo = new ProcessStartInfo(OriginPath, CommandLineOptions);
            if (OriginRunning())  //We must relaunch Origin as a child process for Steam to properly apply the overlay hook.
            {
                ProcessTools.KillProcess("Origin", true, false);
                RestartOrigin = true;
            }
            OriginProcess = Process.Start(originProcessInfo);
            ListenForUnexpectedClose();
        }

        public static bool OriginRunning()
        {
            Process[] pname = Process.GetProcessesByName("Origin");
            if (pname.Length == 0) return false;
            return true;
        }

        private static string GetOriginPath()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\Origin");
            if (key == null) return null;
            return key.GetValue("ClientPath").ToString();
        }

        public void KillOrigin()
        {
            OriginClose(this, new OriginCloseEventArgs(RestartOrigin));
            ProcessTools.KillProcess(OriginProcess, true, false);
            //ProcessTools.KillProcess("sonarhost", false, false);
            ClosedSafely = true;
        }

        public void KillOrigin(int timeout)
        {
            Thread.Sleep(timeout);
            KillOrigin(); 
            
        }

        private async Task ListenForUnexpectedClose()
        {
            await Task.Run(() => OriginProcess.WaitForExit());
            if (!ClosedSafely && OriginUnexpectedClose != null) OriginUnexpectedClose(this);
        }
        public static void CreateUnmanagedInstance()
        {
            ProcessTools.CreateOrphanedProcess(GetOriginPath(), "/StartClientMinimized");
        }
        #endregion

        public static List<DetectedOriginGame> DetectOriginGames()
        {
            if (!Directory.Exists(OriginLocalCacheDirectory))
            {
                return null;
            }

            var gameDirectories = Directory.GetDirectories(OriginLocalCacheDirectory).ToList();

            if (!gameDirectories.Any())
            {
                return null;
            }

            var detectedOriginGames = new List<DetectedOriginGame>();

            foreach (var gameDirectory in gameDirectories)
            {
                var mfstFiles = Directory.GetFiles(gameDirectory, "*.mfst");

                foreach (var mfstFile in mfstFiles)
                {
                    var detectedOriginGame = GetOriginGameFromMfstFile(mfstFile);
                    if (detectedOriginGame != null)
                    {
                        detectedOriginGames.Add(detectedOriginGame);
                    }
                }
            }

            return detectedOriginGames;
        }

        private static DetectedOriginGame GetOriginGameFromMfstFile(string mfstFile)
        {
            var fileContents = File.ReadAllText(mfstFile);
            var nameValues = HttpUtility.ParseQueryString(fileContents);

            var installPath = nameValues.Get("dipinstallpath");
            var id = nameValues.Get("id");

            if (String.IsNullOrEmpty(installPath) || String.IsNullOrEmpty(id))
            {
                return null;
            }

            return new DetectedOriginGame()
            {
                Id = id,
                InstallPath = installPath,
                Name = Path.GetFileName(Path.GetDirectoryName(mfstFile))
            };
        }
    }

    public class OriginCloseEventArgs
    {
        public bool RestartOrigin;

        public OriginCloseEventArgs(bool restartOrigin)
        {
            RestartOrigin = restartOrigin;
        }
    }
}