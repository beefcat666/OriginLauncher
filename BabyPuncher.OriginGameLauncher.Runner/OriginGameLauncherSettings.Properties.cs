using System;
using System.Collections.Generic;
using System.Linq;

namespace BabyPuncher.OriginGameLauncher.Runner
{
    public partial class OriginGameLauncherSettings
    {
        public IDictionary<string, string> SettingsDictionary { get; private set; }

        public string Game
        {
            get { return SettingsDictionary["Game"]; }
            set { SettingsDictionary["Game"] = value ?? string.Empty; }
        }

        public string GameId
        {
            get { return SettingsDictionary["GameId"]; }
            set { SettingsDictionary["GameId"] = value ?? string.Empty; }
        }

        public string GameProcessExe
        {
            get { return SettingsDictionary["GameProcessExe"]; }
            set { SettingsDictionary["GameProcessExe"] = value ?? string.Empty; }
        }
    }
}
