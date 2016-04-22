using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BabyPuncher.OriginGameLauncher.UI.Properties;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class Settings
    {
        private KeyValuePair<string, string> gameKeyValuePair;
        private KeyValuePair<string, string> gameIdKeyValuePair;
        private KeyValuePair<string, string> gameProcessExeKeyValuePair;
        private KeyValuePair<string, string> silentKeyValuePair;

        private static readonly string settingSection = "BabyPuncher.OriginGameLauncher.UI.Properties.Settings";
        
        
        public static Settings GetSettings()
        {
            return new Settings()
            {
                Game = Properties.Settings.Default.Game,
                GameId = Properties.Settings.Default.GameId,
                GameProcessExe = Properties.Settings.Default.GameProcessExe.Trim(),
                Silent = Properties.Settings.Default.Silent
            };
        }

        public void Save()
        {
            saveSettings(settingSection, SettingsDictionary);
        }

        private static void saveSettings(string settingSection, IDictionary<string, string> settings)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var group = config.SectionGroups[@"userSettings"];

            if (group == null) return;

            var clientSection = group.Sections[settingSection] as ClientSettingsSection;
            if (clientSection == null) return;

            var settingElementsToRemove = new List<SettingElement>();
            var settingElementsToAdd = new List<SettingElement>();
            
            settings
                .ToList()
                .ForEach(delegate (KeyValuePair<string, string> setting)
                {
                    foreach (SettingElement settingElement in clientSection.Settings)
                    {
                        if (settingElement.Name == setting.Key)
                        {
                            settingElementsToRemove.Add(settingElement);
                            settingElement.Value.ValueXml.InnerText = setting.Value;
                            settingElementsToAdd.Add(settingElement);
                        }
                    }
                });

            settingElementsToRemove
                .ForEach(delegate (SettingElement setting)
                {
                    clientSection.Settings.Remove(setting);
                });

            settingElementsToAdd
                .ForEach(delegate(SettingElement setting)
                {
                    clientSection.Settings.Add(setting);
                });

            config.Save(ConfigurationSaveMode.Full);
        }
    }
}
