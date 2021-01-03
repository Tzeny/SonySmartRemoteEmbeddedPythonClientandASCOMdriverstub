using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.SonySmartRemoteEmbedded
{
    class SonyCamera
    {
        public int ccdWidth = 1616; // Constants to define the CCD pixel dimensions
        public int ccdHeight = 1080;
        public int previewWidth = 300;
        public int previewHeight = 300;
        public string sensorName = "Sony Alpha Nex5T";
        public SensorType sensorType = SensorType.Color;
        public double pixelSizeX = 4.76; // Constant for the pixel physical dimension
        public double pixelSizeY = 4.76;
        public double exposureMax = 30.0;
        public double exposureMin = 0.0;
        public double exposureTimeStep = 0.1;

        public bool canFastReadout = true;
        public bool imageReadyToDownload = false;

        public bool fastReadout 
        {
            get
            {
                return fastReadout;
            }
            set
            {

            }
        }

        public short readoutMode
        {
            get
            {
                return readoutMode;
            }
            set
            {

            }
        }

        private TraceLogger m_logger;
        public SonyImage lastImage = null;

        public SonyCamera(TraceLogger m_logger)
        {
            this.m_logger = m_logger;

        }

        public void startExposure(double Duration, bool Light)
        {

        }
    }
}
