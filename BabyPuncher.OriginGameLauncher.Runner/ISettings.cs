using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BabyPuncher.OriginGameLauncher.Runner
{
    public interface ISettings
    {
        IDictionary<string, string> SettingsDictionary { get; }
        void Save();
    }
}
