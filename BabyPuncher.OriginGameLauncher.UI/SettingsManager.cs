using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyPuncher.OriginGameLauncher.UI
{
    //Users want to have multiple copies of the app independantly configurable.
    //This class lets us save to App.config while still using .NET config API.
    public class SettingsManager
    {
        public static void SaveUserSetting(string sectionName, string settingName, object settingValue)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var group = config.SectionGroups[@"userSettings"];

            if (group == null) return;
            
            var clientSection = group.Sections[sectionName] as ClientSettingsSection;

            if (clientSection == null) return;
            
            SettingElement settingElement = null;
            
            foreach (SettingElement s in clientSection.Settings)
            {
                if (s.Name == settingName)
                {
                    settingElement = s;
                    break;
                }
            }

            if (settingElement == null) return;
            
            clientSection.Settings.Remove(settingElement);
            
            settingElement.Value.ValueXml.InnerText = settingValue.ToString();
            
            clientSection.Settings.Add(settingElement);
            
            config.Save(ConfigurationSaveMode.Full);
        }
    }
}
