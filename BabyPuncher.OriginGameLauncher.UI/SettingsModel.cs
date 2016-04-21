using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyPuncher.OriginGameLauncher.UI.Properties;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public class SettingsModel
    {
        public string Game { get; set; }
        public string GameId { get; set; }
        public string GameProcessExe { get; set; }
        public bool Silent { get; set; }

        private static readonly string settingSection = "BabyPuncher.OriginGameLauncher.UI.Properties.Settings";

        public static SettingsModel GetSettings()
        {
            return new SettingsModel()
            {
                Game = Settings.Default.Game,
                GameId = Settings.Default.GameId,
                GameProcessExe = Settings.Default.GameProcessExe,
                Silent = Settings.Default.Silent
            };
        }

        public static void SaveSettings(SettingsModel settings)
        {
            SettingsManager.SaveUserSetting(settingSection, "Game", settings.Game);
            SettingsManager.SaveUserSetting(settingSection, "GameId", settings.GameId);
        }
    }
}
