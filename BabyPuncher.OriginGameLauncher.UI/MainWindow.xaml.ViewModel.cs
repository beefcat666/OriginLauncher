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

        public ViewModel(IList<string> detectedOriginGameNames)
        {
            DetectedOriginGameNames = detectedOriginGameNames;
        }
    }
}
