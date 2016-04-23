using System;
using System.Collections.Generic;
using System.Linq;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class OriginLauncherSettings
    {
        public IDictionary<string, string> SettingsDictionary { get; }

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

        public bool Silent
        {
            get { return Boolean.Parse(SettingsDictionary["Silent"]); }
            set { SettingsDictionary["Silent"] = value.ToString(); }
        }
    }
}
