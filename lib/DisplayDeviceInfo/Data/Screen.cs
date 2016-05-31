using System;
using DisplayDeviceInfo.Interfaces;

namespace DisplayDeviceInfo.Data
{
    internal class Screen : IScreen
    {
        public Screen(string name, int width, int height)
        {
            Name = name;
            WidthPx = width;
            HeightPx = height;
        }
        public string Name { get; set;}
        public int WidthPx { get; set; }
        public int HeightPx { get; set;}
        public IDisplayDevice Device { get; set;}
        public IntPtr WindowHandle { get; set;}
    }
}
