using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DisplayDeviceInfo.Interfaces
{
    public interface IDisplayDevice
    {
        string Id { get; set; }
        /// <summary>
        /// Display name such as "///./Display1"
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets/Sets the Width in millimeters
        /// </summary>
        double WidthInMm { get; set; }
        /// <summary>
        /// Gets/Sets the Height in millimeters
        /// </summary>
        double HeightInMm { get; set; }
        /// <summary>
        /// Returns the diagonal monitor size in Inches
        /// </summary>
        /// <returns></returns>
        double GetSize();
        /// <summary>
        /// Returns Width in Inches
        /// </summary>
        /// <returns></returns>
        double GetWidth(double X = 0d, double Y = 0d);
        /// <summary>
        /// /// Returns Height in Inches
        /// </summary>
        /// <returns></returns>
        double GetHeight(double X = 0d, double Y = 0d);

        List<Size> AvailableResolutions { get; set; }
        /// <summary>
        /// Whether the screen has a the physical monitor size read from EDID.
        /// </summary>
        /// <returns></returns>
        bool HasMonitorSize();

        void SetSize(double diagonalSizeInch);

        double GetXPpi(double Xpx, double Ypx);
        double GetYPpi(double xpx, double Ypx);
    }
}
