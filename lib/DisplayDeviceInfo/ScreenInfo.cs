

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using System.Windows.Interop;

using DisplayDeviceInfo.Data;
using DisplayDeviceInfo.Interfaces;
using System.Threading;
using System.Windows.Threading;

namespace DisplayDeviceInfo
{
    public class ScreenInfo
    {
        public static ScreenInfo Display = new ScreenInfo();
        
        public ScreenInfo()
        {
            List = new DisplayDeviceList();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, new MonitorEnumDelegate(HandleMonitorEnum),  IntPtr.Zero);
        }

        public static DisplayDeviceList List 
        {
            get; 
            private set; 
        }

        private IScreen _currentScreen;
        public IScreen CurrentScreen { get { return _currentScreen; } }

        public bool IsPhysicalMonitorDetected()
        {
            return List.Count > 0 && List.Any(d => d.HasMonitorSize() == true);
        }

        public void GetUserInputSize(System.Windows.Window wnd)
        {
            var device = SetCurrentScreen(wnd);
           
            var input = new ManualSizeInputDialog();
            input.ShowDialog();
            string sizeInput = input.InputMonitorSize.Text;
            double size = 0d;
            if (double.TryParse(sizeInput, out size))
            {
                if (device == null)
                {
                    device = new DisplayDevice(size);
                    device.AvailableResolutions.Add(new System.Windows.Size(_currentScreen.WidthPx, _currentScreen.HeightPx));
                    device.Name = _currentScreen.Name;
                }
                else
                {
                    device.SetSize(size);
                }

                if (List.Count == 0)
                {
                    List.Add(device);
                
                }
                _currentScreen.Device = device;
            }
            
        }

        public IDisplayDevice SetCurrentScreen(System.Windows.Window wnd)
        {
            var windowHandle = new WindowInteropHelper(wnd).Handle;
            
            var screen = System.Windows.Forms.Screen.FromHandle(windowHandle);
            IDisplayDevice current = null;
            if (_currentScreen == null || !(_currentScreen.Name.Equals(screen.DeviceName)))
            {
                _currentScreen = new Data.Screen(screen.DeviceName, screen.Bounds.Width, screen.Bounds.Height)
                {
                    WindowHandle = windowHandle
                };

                if (List != null && List.Count > 0)
                {
                    current = List.First(x => x.Id.Equals(_currentScreen.Name));
                       
                    _currentScreen.Device = current;
                    return current;
                }
                    
            }
            else {

                if (_currentScreen.WindowHandle.Equals(windowHandle) || _currentScreen.Name.Equals(screen.DeviceName))
                {
                    current = _currentScreen.Device;
                }
            }
            return current;
        }

        static string ExtractDeviceId(string deviceName)
        {
            var firstSlash = deviceName.IndexOf("\\", StringComparison.Ordinal) + 1;
            var secondSlash = deviceName.IndexOf("\\", firstSlash, StringComparison.Ordinal);
            var result = deviceName.Substring(firstSlash, secondSlash - firstSlash);
            return result;
        }

        delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

        static bool HandleMonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
        {
            MonitorInfoEx mi = new MonitorInfoEx();
            mi.Size = (int)Marshal.SizeOf(mi);
            bool success = GetMonitorInfo(hMonitor, ref mi);

            if (success)
            {
                DisplayInfo di = new DisplayInfo();
                di.ScreenWidth = (mi.Monitor.Right - mi.Monitor.Left).ToString();
                di.ScreenHeight = (mi.Monitor.Bottom - mi.Monitor.Top).ToString();
                // di.MonitorArea = mi.Monitor;
                // di.WorkArea = mi.WorkArea;
                // di.Availability = mi.flags.ToString();
                DISPLAY_DEVICE dev = new DISPLAY_DEVICE();
                var isSucess = DisplayDeviceFromHMonitor(mi, ref dev);

#if NOMONITORDECTED_TEST
                return false;
#endif

                if (isSucess == true)
                {
                    string id = ExtractDeviceId(dev.DeviceID);
                    string deviceId = new string(id.ToCharArray());

                    DisplayDevice monitor = null;
                    
                    /* Get the Size from Device Hardware */
                    int mmWidth = 0; int mmHeight = 0;
                    var getSize = GetSizeForDevID(deviceId, ref mmWidth, ref mmHeight);
                    if (getSize == true)
                    {
                        // The creation of DisplayDevice object in this case means that we've read
                        // the value from EDID.
                        monitor = new DisplayDevice(mmWidth, mmHeight);
                    }
                    else
                    {
                        // Unable to read the EDID value, so we can ask the user to manually
                        // enter it later on.
                        monitor = new DisplayDevice(0D);
                    }
                    
                    /* Get available resolutions */
                    DEVMODE vDevMode = new DEVMODE();
                    int i = 0;
                    List<System.Windows.Size> res = new List<System.Windows.Size>();
                    while (EnumDisplaySettings(mi.DeviceName, i, ref vDevMode))
                    {
                        var f = res.Find(x => x.Width.Equals(double.Parse(vDevMode.dmPelsWidth.ToString())));
                        if (f.Width == 0D && f.Height == 0D)
                        {
                            res.Add(new System.Windows.Size((double)vDevMode.dmPelsWidth, (double)vDevMode.dmPelsHeight));
                        }
                        i++;
                    }
                    if (res.Count > 0)
                    {
                        monitor.AvailableResolutions = res;
                    }

                    monitor.Id = mi.DeviceName;
                    monitor.Name = deviceId;

                    /* Add it to the list*/
                    List.Add(monitor);
                }
                else
                {
                    //MessageBox.Show("Method: DisplayDeviceFromHMonitor()", "Unsuccessful!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        /***********************************************************************************************************/
        [DllImport("EdidInfo.dll", EntryPoint = "GetSizeForDevID", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        static extern bool GetSizeForDevID(string targetDeviceId, ref int widthMm, ref int heightMm);

        [DllImport("EdidInfo.dll", EntryPoint = "DisplayDeviceFromHMonitor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        static extern bool DisplayDeviceFromHMonitor(MonitorInfoEx mi, ref DISPLAY_DEVICE ddMonOut);
        
        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        [DllImport("user32.dll")]
        static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        /***********************************************************************************************************/

        const int ENUM_CURRENT_SETTINGS = -1;

        const int ENUM_REGISTRY_SETTINGS = -2;

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
    
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        public struct DISPLAY_DEVICE 
        {
              [MarshalAs(UnmanagedType.U4)]
              public int cb;
              [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
              public string DeviceName;
              [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
              public string DeviceString;
              [MarshalAs(UnmanagedType.U4)]
              public DisplayDeviceStateFlags StateFlags;
              [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
              public string DeviceID;
              [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
              public string DeviceKey;
        }

        [Flags()]
        public enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VgaCompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MonitorInfo
        {
            public uint size;
            public Rect monitor;
            public Rect work;
            public uint flags;
        }

        public class DisplayInfo
        {
            public string Availability { get; set; }
            public string ScreenHeight { get; set; }
            public string ScreenWidth { get; set; }
            public Rect MonitorArea { get; set; }
            public Rect WorkArea { get; set; }
        }

        // size of a device name string
        private const int CCHDEVICENAME = 32;

        /// <summary>
        /// The MONITORINFOEX structure contains information about a display monitor.
        /// The GetMonitorInfo function stores information into a MONITORINFOEX structure or a MONITORINFO structure.
        /// The MONITORINFOEX structure is a superset of the MONITORINFO structure. The MONITORINFOEX structure adds a string member to contain a name 
        /// for the display monitor.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct MonitorInfoEx
        {
            /// <summary>
            /// The size, in bytes, of the structure. Set this member to sizeof(MONITORINFOEX) (72) before calling the GetMonitorInfo function. 
            /// Doing so lets the function determine the type of structure you are passing to it.
            /// </summary>
            public int Size;

            /// <summary>
            /// A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates. 
            /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
            /// </summary>
            public RectStruct Monitor;

            /// <summary>
            /// A RECT structure that specifies the work area rectangle of the display monitor that can be used by applications, 
            /// expressed in virtual-screen coordinates. Windows uses this rectangle to maximize an application on the monitor. 
            /// The rest of the area in rcMonitor contains system windows such as the task bar and side bars. 
            /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
            /// </summary>
            public RectStruct WorkArea;

            /// <summary>
            /// The attributes of the display monitor.
            /// 
            /// This member can be the following value:
            ///   1 : MONITORINFOF_PRIMARY
            /// </summary>
            public uint Flags;

            /// <summary>
            /// A string that specifies the device name of the monitor being used. Most applications have no use for a display monitor name, 
            /// and so can save some bytes by using a MONITORINFO structure.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string DeviceName;

            public void Init()
            {
                this.Size = 40 + 2 * CCHDEVICENAME;
                this.DeviceName = string.Empty;
            }
        }

        /// <summary>
        /// The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/dd162897%28VS.85%29.aspx"/>
        /// <remarks>
        /// By convention, the right and bottom edges of the rectangle are normally considered exclusive. 
        /// In other words, the pixel whose coordinates are ( right, bottom ) lies immediately outside of the the rectangle. 
        /// For example, when RECT is passed to the FillRect function, the rectangle is filled up to, but not including, 
        /// the right column and bottom row of pixels. This structure is identical to the RECTL structure.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct RectStruct
        {
            /// <summary>
            /// The x-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public int Left;

            /// <summary>
            /// The y-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public int Top;

            /// <summary>
            /// The x-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public int Right;

            /// <summary>
            /// The y-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public int Bottom;

        }
    }
}
