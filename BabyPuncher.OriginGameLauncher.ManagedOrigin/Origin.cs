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
        private bool restartOrigin;
        private bool closedSafely;

        private static readonly string originLocalCacheDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Origin\\LocalContent";

        public string OriginPath;
        public string CommandLineOptions;

        public Process OriginProcess;

        public delegate void OriginCloseEvent(OriginCloseEventArgs e);
        public event OriginCloseEvent OriginClose;

        public delegate void OriginUnexpectedCloseEvent();
        public event OriginUnexpectedCloseEvent OriginUnexpectedClose;
        
        public Origin(string commandLineOptions)
        {
            CommandLineOptions = commandLineOptions;
            OriginClose += origin_OriginClose;
        }

        public Origin() : this("/StartClientMinimized") { } //Minimized Origin by default

        #region Origin
        private void origin_OriginClose(OriginCloseEventArgs args)
        {
            if (args.RestartOrigin) CreateUnmanagedInstance();
        }

        public void StartOrigin()
        {
            OriginPath = getOriginPath();
            if (OriginPath == null) return;
            var originProcessInfo = new ProcessStartInfo(OriginPath, CommandLineOptions);
            if (IsOriginRunning())  //We must relaunch Origin as a child process for Steam to properly apply the overlay hook.
            {
                ProcessTools.KillProcess("Origin", true, false);
                restartOrigin = true;
            }
            OriginProcess = Process.Start(originProcessInfo);
            listenForUnexpectedClose();
        }

        public static bool IsOriginRunning()
        {
            Process[] pname = Process.GetProcessesByName("Origin");
            if (pname.Length == 0) return false;
            return true;
        }

        private static string getOriginPath()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\Origin");
            if (key == null) return null;
            return key.GetValue("ClientPath").ToString();
        }

        public void KillOrigin()
        {
            if (IsOriginRunning())
            {
                OriginClose(new OriginCloseEventArgs(restartOrigin));
                ProcessTools.KillProcess(OriginProcess, true, false);
                closedSafely = true;
            }
        }

        public void KillOrigin(int timeout)
        {
            Thread.Sleep(timeout);
            KillOrigin(); 
            
        }

        private async void listenForUnexpectedClose()
        {
            await Task.Run(() => OriginProcess.WaitForExit());
            if (!closedSafely && OriginUnexpectedClose != null) OriginUnexpectedClose();
        }

        public static void CreateUnmanagedInstance()
        {
            ProcessTools.CreateOrphanedProcess(getOriginPath(), "/StartClientMinimized");
        }
        #endregion

        public static List<OriginGame> DetectOriginGames()
        {
            if (!Directory.Exists(originLocalCacheDirectory))
            {
                return null;
            }

            var gameDirectories = Directory.GetDirectories(originLocalCacheDirectory).ToList();

            if (!gameDirectories.Any())
            {
                return null;
            }

            var detectedOriginGames = new List<OriginGame>();

            gameDirectories.ForEach(gameDirectory =>
            {
                var mfstFiles = Directory.GetFiles(gameDirectory, "*.mfst");

                mfstFiles.ToList().ForEach(mfstFile =>
                {
                    var detectedOriginGame = getOriginGameFromMfstFile(mfstFile);
                    if (detectedOriginGame != null)
                    {
                        detectedOriginGames.Add(detectedOriginGame);
                    }
                });
            });

            return detectedOriginGames;
        }

        private static OriginGame getOriginGameFromMfstFile(string mfstFile)
        {
            var fileContents = File.ReadAllText(mfstFile);
            var nameValues = HttpUtility.ParseQueryString(fileContents);

            var installPath = nameValues.Get("dipinstallpath");
            var id = nameValues.Get("id");

            if (String.IsNullOrEmpty(installPath) || String.IsNullOrEmpty(id))
            {
                return null;
            }

            return new OriginGame()
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