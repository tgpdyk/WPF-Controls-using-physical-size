using DisplayDeviceInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayDeviceInfo.Data
{
    internal class DisplayDevice : IDisplayDevice
    {
        readonly double CmToInch = 0.393701;
        readonly double MmToCm = 0.1;

        public string Id { get; set;}
        public string Name { get; set;}
        public double WidthInMm { get; set; }
        public double HeightInMm { get; set; }
        public List<System.Windows.Size> AvailableResolutions { get; set; }

        private bool IsSizeComingFromEdid { get; set; }
        private bool IsSizeSetAlready { get; set; }

        public DisplayDevice(double mmWidth, double mmHeight)
        {
            AvailableResolutions = new List<System.Windows.Size>();
            IsSizeSetAlready = false;
            IsSizeComingFromEdid = true;
            // Width and Height should not contain zero value
            if (mmWidth.Equals(0d) || mmHeight.Equals(0d))
            {
                // To force user to enter the actual monitor size.
                IsSizeComingFromEdid = false;
            }
            else
            {
                WidthInMm = mmWidth;
                HeightInMm = mmHeight;
            }
        }

        public DisplayDevice(double diagonalSizeInch)
        {
            AvailableResolutions = new List<System.Windows.Size>();
            IsSizeSetAlready = true;
            IsSizeComingFromEdid = false;
            _diagonalSize = diagonalSizeInch;
        }

        public bool HasMonitorSize()
        {
            if (IsSizeSetAlready == true)
            {
                return true;
            }
            return IsSizeComingFromEdid;
        }

        public double GetSize()
        {
            if (IsSizeComingFromEdid == true)
            {
                //Pythagorean 
                var a = Math.Pow(this.GetWidth(), 2);
                var b = Math.Pow(this.GetHeight(), 2);
                var c = Math.Sqrt(a + b);
                return c;
            }
            else {
                return _diagonalSize;
            }
        }

        private double _diagonalSize;
        public void SetSize(double diagonalSizeInch)
        {
            _diagonalSize = diagonalSizeInch;
            IsSizeSetAlready = true;
        }

        public double GetWidth(double X = 0d, double Y = 0d)
        {
            double w = 0d;
            if (WidthInMm > 0 && IsSizeComingFromEdid == true)
            {
                w = (WidthInMm * MmToCm) * CmToInch;
            }
            else
            {
                var aspectRatio = Y / X;
                var atan = Math.Atan(aspectRatio);
                w = Math.Cos(atan) * _diagonalSize;
                
            }
            return w;
        }


        public double GetHeight(double X = 0d, double Y = 0d)
        {
            double h = 0d;
            if (HeightInMm > 0 && IsSizeComingFromEdid == true)
            {
                h = (HeightInMm * MmToCm) * CmToInch;
            }
            else
            {
                var aspectRatio = Y / X;
                var atan = Math.Atan(aspectRatio);
                h = Math.Sin(atan) * _diagonalSize;
            }
            return h;
        }

        public double GetYPpi(double xpx, double Ypx) 
        {
            if (AvailableResolutions.Count > 0)
            {
                // This is assumption: The highest resolution is the Native resolution.
                // Problem: How to know if the a particular res is native or preferred/recommended?
                var nativeHeight = AvailableResolutions.Max(x => x.Height);
                // possible problem here: when there are res like this: 1680x1050, 1680x980
                if (nativeHeight > 0d)
                {
                    var nativeRes = AvailableResolutions.Find(x => double.Equals(x.Height, nativeHeight));
                    if (nativeRes.Width > 0d && nativeRes.Height > 0d)
                    {
                        // current screen resolution
                        double width = xpx;
                        double height = Ypx;
                        double y = 0d;
                        
                        //if (height.Equals((int)nativeRes.Height))
                        //{
                        //    y = height / GetHeight(width, height);
                        //}
                        //else
                        //{
                        var nativePpi = nativeRes.Height / GetHeight();
                        double aspectRatio = width / height;
                        double scaledHeight = (nativeRes.Width / aspectRatio) / nativePpi;
                        y = height / scaledHeight;
                        //}

                        if (y > 0d)
                        {
                            return y;
                        }
                    }
                }
            }
            else
            {
                Debug.Assert(false, "Current screen or Available resolutions have no value. Cannot compute the PPI");
            }
            return 0d;
        }

        public double GetXPpi(double Xpx, double Ypx)
        {
            if (AvailableResolutions.Count > 0)
            {
                // This is an assumption: The highest resolution is the Native resolution.
                // Problem: How to know if the a particular res is native or preferred/recommended?
                var nativeWidth = AvailableResolutions.Max(x => x.Width);
                // possible problem here: when there are res like this: 1680x1050, 1680x980
                if (nativeWidth > 0d)
                {
                    var nativeRes = AvailableResolutions.Find(x => double.Equals(x.Width, nativeWidth));
                    if (nativeRes.Width > 0d && nativeRes.Height > 0d)
                    {
                        // current screen resolution
                        double width = Xpx;
                        double height = Ypx;
                        double x = 0d;
                        //if (width.Equals((int)nativeRes.Width))
                        //{
                        //    x = width / GetWidth(width, height);
                        //}
                        //else
                        //{
                        var nativePpi = nativeRes.Width / GetWidth(width, height);
                        double aspectRatio = width / height;
                        double scaledWidth = (aspectRatio * nativeRes.Height) / nativePpi;
                        x = width / scaledWidth;
                       // }

                        if (x > 0d)
                        {
                            return x;
                        }
                    }
                }
            }
            else {
                Debug.Assert(false, "Current screen or Available resolutions are no value. Cannot compute the PPI");
            }
            return 0d;
        }
    }
}
