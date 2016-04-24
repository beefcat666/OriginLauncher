using System;
using System.IO;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public partial class MainWindow
    {
        private static void createBlankConfigFile()
        {
            var blankConfig =
  @"<?xml version=""1.0"" encoding=""utf-8"" ?>" + Environment.NewLine
+ @"<configuration>" + Environment.NewLine
+ @"    <configSections>" + Environment.NewLine
+ @"        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" >" + Environment.NewLine
+ @"            <section name=""BabyPuncher.OriginGameLauncher.Runner.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />" + Environment.NewLine
+ @"        </sectionGroup>" + Environment.NewLine
+ @"    </configSections>" + Environment.NewLine
+ @"    <startup> " + Environment.NewLine
+ @"        <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.5.2"" />" + Environment.NewLine
+ @"    </startup>" + Environment.NewLine
+ @"    <userSettings>" + Environment.NewLine
+ @"        <BabyPuncher.OriginGameLauncher.Runner.Properties.Settings>" + Environment.NewLine
+ @"            <setting name=""Game"" serializeAs=""String"">" + Environment.NewLine
+ @"                <value></value>" + Environment.NewLine
+ @"            </setting>" + Environment.NewLine
+ @"            <setting name=""GameId"" serializeAs=""String"">" + Environment.NewLine
+ @"                <value></value>" + Environment.NewLine
+ @"            </setting>" + Environment.NewLine
+ @"            <setting name=""GameProcessExe"" serializeAs=""String"">" + Environment.NewLine
+ @"                <value></value>" + Environment.NewLine
+ @"            </setting>" + Environment.NewLine
+ @"        </BabyPuncher.OriginGameLauncher.Runner.Properties.Settings>" + Environment.NewLine
+ @"    </userSettings>" + Environment.NewLine
+ @"</configuration>";

            File.WriteAllText(configFileName, blankConfig);
        }

    }
}
