using System;

namespace DisplayDeviceInfo.Interfaces
{
    public interface IScreen
    {
        string Name { get; set; }
        int WidthPx { get; set; }
        int HeightPx { get; set; }
        IDisplayDevice Device { get; set; }
        IntPtr WindowHandle { get; set; }
    }
}
