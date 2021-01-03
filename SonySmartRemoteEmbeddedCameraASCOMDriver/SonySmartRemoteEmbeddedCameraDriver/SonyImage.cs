using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.SonySmartRemoteEmbedded
{
    class SonyImage
    {
        public bool isReady = false;
        public double exposureDuration = 0.0;
        public DateTime startTime = DateTime.Now;
        public int[,,] pixels;

        public SonyImage()
        {

        }

        public int[,,] getPixels()
        {
            return null;
        }
    }
}
