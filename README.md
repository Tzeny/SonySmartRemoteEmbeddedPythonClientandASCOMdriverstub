# Sony Smart Remote Embedded Python Client and ASCOM driver stub
Sony offers a Smart Remote Embedded application for some of their cameras. The application creates a WiFi hotspot through which the camera's functions can be controlled. I wanted to use my Sony NEX-5T for astrophotography so I reverse engineered the API in Python. 
I started writing a C# ASCOM driver for it. I gave up on the driver half way when I realized that the captured images are not full resolution and that not RAW (or ARW in my camera's case) files are produced, which means I can't properly use it for astrophotography. Thanks to https://github.com/dougforpres/ASCOMSonyCameraDriver for pointing me in the right direction with the driver.

**Note: the Python code was only tested on my Sony NEX-5T.** It should work with all the models supported by the application (DSC-HX400V DSC-HX60V DSC-HX80 DSC-HX90V DSC-RX100M3 DSC-RX100M4 DSC-RX100M5 DSC-RX10M2 DSC-RX10M3 DSC-RX1RM2 DSC-WX500 ILCE-5000 ILCE-5100 ILCE-6000 ILCE-6300 ILCE-6500 ILCE-7 ILCE-7M2 ILCE-7R ILCE-7RM2 ILCE-7S ILCE-7SM2 NEX-5R NEX-5T NEX-6) [https://www.playmemoriescameraapps.com/portal/usbdetail.php?eid=is9104-npia09014_00-f00002] but I can't be sure. 
## 

## Python Client
I wrote a short example 'API' using Python's *requests* library and *Pillow* in order to demonstrate how to call the camera's API.
In order to use the Python Client in the Jupyter Notebook you have to install Pillow (pip install Pillow) and use Python 3+.
The Notebook has comments explaining the requests.

**Note: if you have a different camera model you have to perform an M-SEARCH request (M-SEARCH MAN: "ssdp:discover" MX: 2 ST: ssdp:all) in order to find the IP and the API location**

## ASCOM Driver
I started developing an ASCOM C# driver based on their official Developer Tools (https://ascom-standards.org/Downloads/DevTools.htm). The driver is not complete. The SonyCamera.cs and SonyImage.cs components need to be completed in order for it to work. The Visual Studio solution is located in the SonySmartRemoteEmbeddedCameraASCOMDriver folder.

**Note: for 0.0 second exposures return frames from the liveview**

## API Traffic Capture
To decipher the API I captured the traffic between Sony's Imaging Edge Mobile application (https://play.google.com/store/apps/details?id=com.sony.playmemories.mobile&hl=en&gl=US) and the camera. pcap files and an explanation are available in the APITrafficCapture folder.
