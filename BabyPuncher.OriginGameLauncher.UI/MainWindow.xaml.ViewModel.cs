using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyPuncher.OriginGameLauncher.UI
{
    public class ViewModel
    {
        public IList<string> DetectedOriginGameNames { get; set; }
        public string GameExeFileName { get; set; }
        public string Game { get; set; }
    }
}
