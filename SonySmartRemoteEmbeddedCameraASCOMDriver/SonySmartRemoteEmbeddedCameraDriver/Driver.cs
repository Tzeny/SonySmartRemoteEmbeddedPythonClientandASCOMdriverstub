//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Camera driver for SonySmartRemoteEmbedded
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Camera interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//


// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.
#define Camera

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;

namespace ASCOM.SonySmartRemoteEmbedded
{
    //
    // Your driver's DeviceID is ASCOM.SonySmartRemoteEmbedded.Camera
    //
    // The Guid attribute sets the CLSID for ASCOM.SonySmartRemoteEmbedded.Camera
    // The ClassInterface/None attribute prevents an empty interface called
    // _SonySmartRemoteEmbedded from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Camera Driver for SonySmartRemoteEmbedded.
    /// </summary>
    [Guid("fec59b3b-6872-47f3-a026-392168d86c59")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Camera : ICameraV3
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.SonySmartRemoteEmbedded.Camera";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "ASCOM Camera Driver for SonySmartRemoteEmbedded.";

        internal static string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        internal static string comPortDefault = "COM1";
        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";

        internal static string comPort; // Variables to hold the current device configuration

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal TraceLogger tl;

        private SonyCamera camera;

        private int ccdWidth; // Constants to define the CCD pixel dimensions
        private int ccdHeight;

        private int cameraNumX; // Initialise variables to hold values required for functionality tested by Conform
        private int cameraNumY;

        /// <summary>
        /// Initializes a new instance of the <see cref="SonySmartRemoteEmbedded"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Camera()
        {
            tl = new TraceLogger("", "SonySmartRemoteEmbedded");
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl.LogMessage("Camera", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro-utilities object
            //TODO: Implement your additional construction here

            tl.LogMessage("Camera", "Completed initialisation");

            camera = new SonyCamera(tl);

            ccdWidth = camera.ccdWidth; // Constants to define the CCD pixel dimensions
            ccdHeight = camera.ccdWidth;

            cameraNumX = ccdWidth;
            cameraNumY = ccdHeight;
        }


        //
        // PUBLIC COM INTERFACE ICameraV3 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(tl))
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBlind must send the supplied command to the mount and return immediately without waiting for a response

            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            // TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

            // string retString = CommandString(command, raw); // Send the command and wait for the response
            // bool retBool = XXXXXXXXXXXXX; // Parse the returned string and create a boolean True / False value
            // return retBool; // Return the boolean value to the client

            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the trace logger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;

                if (value)
                {
                    connectedState = true;
                    LogMessage("Connected Set", "Connecting to port {0}", comPort);
                    // TODO connect to the device
                }
                else
                {
                    connectedState = false;
                    LogMessage("Connected Set", "Disconnecting from port {0}", comPort);
                    // TODO disconnect from the device
                }
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "3");
                return Convert.ToInt16("3");
            }
        }

        public string Name
        {
            get
            {
                string name = "Short driver name - please customise";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region ICamera Implementation

        private int cameraStartX = 0;
        private int cameraStartY = 0;
        private DateTime exposureStart = DateTime.MinValue;
        private double cameraLastExposureDuration = 0.0;
        private bool cameraImageReady = false;
        private int[,] cameraImageArray;
        private object[,] cameraImageArrayVariant;

        public void AbortExposure()
        {
            tl.LogMessage("AbortExposure", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("AbortExposure");
        }

        public short BayerOffsetX
        {
            get
            {
                tl.LogMessage("BayerOffsetX Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("BayerOffsetX", false);
            }
        }

        public short BayerOffsetY
        {
            get
            {
                tl.LogMessage("BayerOffsetY Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("BayerOffsetX", true);
            }
        }

        public short BinX
        {
            get
            {
                tl.LogMessage("BinX Get", "1");
                return 1;
            }
            set
            {
                tl.LogMessage("BinX Set", value.ToString());
                if (value != 1) throw new ASCOM.InvalidValueException("BinX", value.ToString(), "1"); // Only 1 is valid in this simple template
            }
        }

        public short BinY
        {
            get
            {
                tl.LogMessage("BinY Get", "1");
                return 1;
            }
            set
            {
                tl.LogMessage("BinY Set", value.ToString());
                if (value != 1) throw new ASCOM.InvalidValueException("BinY", value.ToString(), "1"); // Only 1 is valid in this simple template
            }
        }

        public double CCDTemperature
        {
            get
            {
                tl.LogMessage("CCDTemperature Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("CCDTemperature", false);
            }
        }

        public CameraStates CameraState
        {
            get
            {
                tl.LogMessage("CameraState Get", CameraStates.cameraIdle.ToString());
                return CameraStates.cameraIdle;
            }
        }

        public int CameraXSize
        {
            get
            {
                tl.LogMessage("CameraXSize Get", ccdWidth.ToString());
                return ccdWidth;
            }
        }

        public int CameraYSize
        {
            get
            {
                tl.LogMessage("CameraYSize Get", ccdHeight.ToString());
                return ccdHeight;
            }
        }

        public bool CanAbortExposure
        {
            get
            {
                tl.LogMessage("CanAbortExposure Get", false.ToString());
                return false;
            }
        }

        public bool CanAsymmetricBin
        {
            get
            {
                tl.LogMessage("CanAsymmetricBin Get", false.ToString());
                return false;
            }
        }

        public bool CanFastReadout
        {
            get
            {
                tl.LogMessage("CanFastReadout Get", camera.canFastReadout.ToString());
                return camera.canFastReadout;
            }
        }

        public bool CanGetCoolerPower
        {
            get
            {
                tl.LogMessage("CanGetCoolerPower Get", false.ToString());
                return false;
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                tl.LogMessage("CanPulseGuide Get", false.ToString());
                return false;
            }
        }

        public bool CanSetCCDTemperature
        {
            get
            {
                tl.LogMessage("CanSetCCDTemperature Get", false.ToString());
                return false;
            }
        }

        public bool CanStopExposure
        {
            get
            {
                tl.LogMessage("CanStopExposure Get", false.ToString());
                return false;
            }
        }

        public bool CoolerOn
        {
            get
            {
                tl.LogMessage("CoolerOn Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("CoolerOn", false);
            }
            set
            {
                tl.LogMessage("CoolerOn Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("CoolerOn", true);
            }
        }

        public double CoolerPower
        {
            get
            {
                tl.LogMessage("CoolerPower Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("CoolerPower", false);
            }
        }

        public double ElectronsPerADU
        {
            get
            {
                tl.LogMessage("ElectronsPerADU Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ElectronsPerADU", false);
            }
        }

        public double ExposureMax
        {
            get
            {
                tl.LogMessage("ExposureMax Get Get", camera.exposureMax.ToString());

                return camera.exposureMax;
            }
        }

        public double ExposureMin
        {
            get
            {
                tl.LogMessage("ExposureMax Get Get", camera.exposureMin.ToString());

                return camera.exposureMin;
            }
        }

        public double ExposureResolution
        {
            get
            {
                tl.LogMessage("ExposureMax Get Get", camera.exposureTimeStep.ToString());

                return camera.exposureTimeStep;
            }
        }

        public bool FastReadout
        {
            get
            {
                tl.LogMessage("FastReadout Get", camera.fastReadout.ToString());

                return camera.fastReadout;
            }
            set
            {
                camera.fastReadout = value;
            }
        }

        public double FullWellCapacity
        {
            get
            {
                tl.LogMessage("FullWellCapacity Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("FullWellCapacity", false);
            }
        }

        public short Gain
        {
            get
            {
                tl.LogMessage("Gain Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Gain", false);
            }
            set
            {
                tl.LogMessage("Gain Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Gain", true);
            }
        }

        public short GainMax
        {
            get
            {
                tl.LogMessage("GainMax Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GainMax", false);
            }
        }

        public short GainMin
        {
            get
            {
                tl.LogMessage("GainMin Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GainMin", true);
            }
        }

        public ArrayList Gains
        {
            get
            {
                tl.LogMessage("Gains Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Gains", true);
            }
        }

        public bool HasShutter
        {
            get
            {
                tl.LogMessage("HasShutter Get", true.ToString());
                return true;
            }
        }

        public double HeatSinkTemperature
        {
            get
            {
                tl.LogMessage("HeatSinkTemperature Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("HeatSinkTemperature", false);
            }
        }

        public object ImageArray
        {
            get
            {
                object result = null; //int[,,]

                if (camera.lastImage is null || !camera.lastImage.isReady)
                {
                    tl.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken or before the last image is ready!");
                    throw new ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken or before the last image is ready!");
                }

                return camera.lastImage.getPixels();
            }
        }

        public object ImageArrayVariant
        {
            get
            {
                //if (!cameraImageReady)
                //{
                //    tl.LogMessage("ImageArrayVariant Get", "Throwing InvalidOperationException because of a call to ImageArrayVariant before the first image has been taken!");
                //    throw new ASCOM.InvalidOperationException("Call to ImageArrayVariant before the first image has been taken!");
                //}
                //cameraImageArrayVariant = new object[cameraNumX, cameraNumY];
                //for (int i = 0; i < cameraImageArray.GetLength(1); i++)
                //{
                //    for (int j = 0; j < cameraImageArray.GetLength(0); j++)
                //    {
                //        cameraImageArrayVariant[j, i] = cameraImageArray[j, i];
                //    }

                //}

                //return cameraImageArrayVariant;

                if (camera.lastImage is null || !camera.lastImage.isReady)
                {
                    tl.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken or before the last image is ready!");
                    throw new ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken or before the last image is ready!");
                }
                object[,,] cameraImageArrayVariant = new object[cameraNumX, cameraNumY, 3];

                return cameraImageArrayVariant;
            }
        }

        public bool ImageReady
        {
            get
            {
                tl.LogMessage("ImageReady Get", camera.imageReadyToDownload.ToString());
                return camera.imageReadyToDownload;
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                tl.LogMessage("IsPulseGuiding Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("IsPulseGuiding", false);
            }
        }

        public double LastExposureDuration
        {
            get
            {
                if (camera.lastImage is null || !camera.lastImage.isReady)
                {
                    tl.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken or before the last image is ready!");
                    throw new ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken or before the last image is ready!");
                }
                tl.LogMessage("LastExposureDuration Get", camera.lastImage.exposureDuration.ToString());
                return camera.lastImage.exposureDuration;
            }
        }

        public string LastExposureStartTime
        {
            get
            {
                if (camera.lastImage is null || !camera.lastImage.isReady)
                {
                    tl.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken or before the last image is ready!");
                    throw new ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken or before the last image is ready!");
                }

                string exposureStartString = camera.lastImage.startTime.ToString("yyyy-MM-ddTHH:mm:ss");
                tl.LogMessage("LastExposureStartTime Get", exposureStartString.ToString());
                return exposureStartString;
            }
        }

        public int MaxADU
        {
            get
            {
                tl.LogMessage("MaxADU Get", "20000");
                return 20000;
            }
        }

        public short MaxBinX
        {
            get
            {
                tl.LogMessage("MaxBinX Get", "1");
                return 1;
            }
        }

        public short MaxBinY
        {
            get
            {
                tl.LogMessage("MaxBinY Get", "1");
                return 1;
            }
        }

        public int NumX
        {
            get
            {
                tl.LogMessage("NumX Get", cameraNumX.ToString());
                return cameraNumX;
            }
            set
            {
                cameraNumX = value;
                tl.LogMessage("NumX set", value.ToString());
            }
        }

        public int NumY
        {
            get
            {
                tl.LogMessage("NumY Get", cameraNumY.ToString());
                return cameraNumY;
            }
            set
            {
                cameraNumY = value;
                tl.LogMessage("NumY set", value.ToString());
            }
        }

        public int Offset
        {
            get
            {
                tl.LogMessage("Offset Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Offset", false);
            }
            set
            {
                tl.LogMessage("Offset Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Offset", true);
            }
        }

        public int OffsetMax
        {
            get
            {
                tl.LogMessage("OffsetMax Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("OffsetMax", false);
            }
        }

        public int OffsetMin
        {
            get
            {
                tl.LogMessage("OffsetMin Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("OffsetMin", true);
            }
        }

        public ArrayList Offsets
        {
            get
            {
                tl.LogMessage("Offsets Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Offsets", true);
            }
        }

        public short PercentCompleted
        {
            get
            {
                tl.LogMessage("PercentCompleted Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("PercentCompleted", false);
            }
        }

        public double PixelSizeX
        {
            get
            {
                tl.LogMessage("PixelSizeX Get", camera.pixelSizeX.ToString());
                return camera.pixelSizeX;
            }
        }

        public double PixelSizeY
        {
            get
            {
                tl.LogMessage("PixelSizeY Get", camera.pixelSizeY.ToString());
                return camera.pixelSizeY;
            }
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            tl.LogMessage("PulseGuide", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("PulseGuide");
        }

        public short ReadoutMode
        {
            get
            {
                tl.LogMessage("ReadoutMode get", camera.readoutMode.ToString());

                return camera.readoutMode;
            }
            set
            {
                if (ReadoutModes.Count > value)
                {
                    tl.LogMessage("ReadoutMode Set", value.ToString());

                    camera.readoutMode = value;
                }
                else
                {
                    throw new ASCOM.InvalidValueException("ReadoutMode not in allowable values");
                }
            }
        }

        public ArrayList ReadoutModes
        {
            get
            {
                ArrayList modes = new ArrayList();

                modes.Add(String.Format("Full Resolution ({0} x {1})", camera.ccdWidth, camera.ccdHeight));
                modes.Add(String.Format("LiveView ({0} x {1})", camera.previewWidth, camera.previewHeight));

                return modes;
            }
        }

        public string SensorName
        {
            get
            {
                tl.LogMessage("SensorName Get", camera.sensorName);
                return camera.sensorName;
            }
        }

        public SensorType SensorType
        {
            get
            {
                tl.LogMessage("SensorType Get", camera.sensorType.ToString());
                return camera.sensorType;
            }
        }

        public double SetCCDTemperature
        {
            get
            {
                tl.LogMessage("SetCCDTemperature Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SetCCDTemperature", false);
            }
            set
            {
                tl.LogMessage("SetCCDTemperature Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SetCCDTemperature", true);
            }
        }

        public void StartExposure(double Duration, bool Light)
        {
            if (Duration < 0.0) throw new InvalidValueException("StartExposure", Duration.ToString(), "0.0 upwards");
            if (cameraNumX > ccdWidth) throw new InvalidValueException("StartExposure", cameraNumX.ToString(), ccdWidth.ToString());
            if (cameraNumY > ccdHeight) throw new InvalidValueException("StartExposure", cameraNumY.ToString(), ccdHeight.ToString());
            if (cameraStartX > ccdWidth) throw new InvalidValueException("StartExposure", cameraStartX.ToString(), ccdWidth.ToString());
            if (cameraStartY > ccdHeight) throw new InvalidValueException("StartExposure", cameraStartY.ToString(), ccdHeight.ToString());

            cameraLastExposureDuration = Duration;

            tl.LogMessage("StartExposure", String.Format("Duration={0}, Light={1}", Duration.ToString(), Light.ToString()));

            exposureStart = DateTime.Now;
            camera.startExposure(Duration, Light);
        }

        public int StartX
        {
            get
            {
                tl.LogMessage("StartX Get", cameraStartX.ToString());
                return cameraStartX;
            }
            set
            {
                cameraStartX = value;
                tl.LogMessage("StartX Set", value.ToString());
            }
        }

        public int StartY
        {
            get
            {
                tl.LogMessage("StartY Get", cameraStartY.ToString());
                return cameraStartY;
            }
            set
            {
                cameraStartY = value;
                tl.LogMessage("StartY set", value.ToString());
            }
        }

        public void StopExposure()
        {
            tl.LogMessage("StopExposure", "Not implemented");
            throw new MethodNotImplementedException("StopExposure");
        }

        public double SubExposureDuration
        {
            get
            {
                tl.LogMessage("SubExposureDuration Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SubExposureDuration", false);
            }
            set
            {
                tl.LogMessage("SubExposureDuration Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SubExposureDuration", true);
            }
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Camera";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Camera";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Camera";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
