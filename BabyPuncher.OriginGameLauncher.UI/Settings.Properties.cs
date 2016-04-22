using System;
using System.Collections.Generic;
using System.Linq;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class Settings
    {
        public IDictionary<string, string> SettingsDictionary
        {
            get
            {
                return new List<KeyValuePair<string, string>>()
                {
                    gameKeyValuePair,
                    gameIdKeyValuePair,
                    gameProcessExeKeyValuePair,
                    silentKeyValuePair
                }.ToDictionary(k => k.Key, v => v.Value);
            }
        }

        public string Game
        {
            get { return gameKeyValuePair.Value; }
            set { gameKeyValuePair = new KeyValuePair<string, string>("Game", value ?? string.Empty); }
        }

        public string GameId
        {
            get { return gameIdKeyValuePair.Value; }
            set { gameIdKeyValuePair = new KeyValuePair<string, string>("GameId", value ?? string.Empty); }
        }

        public string GameProcessExe
        {
            get { return gameProcessExeKeyValuePair.Value; }
            set { gameProcessExeKeyValuePair = new KeyValuePair<string, string>("GameProcessExe", value ?? string.Empty); }
        }

        public bool Silent
        {
            get { return Boolean.Parse(silentKeyValuePair.Value); }
            set { silentKeyValuePair = new KeyValuePair<string, string>("Silent", value.ToString()); }
        }
    }
}
