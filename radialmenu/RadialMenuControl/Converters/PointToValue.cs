using System.Windows;
using System.Windows.Controls;
using RadialMenuControl.Common;
using RadialMenuControl.Views.Gauge;

namespace RadialMenuControl.Converters
{
    internal class PointToValue : MultiBindingConverterMarkupExtension<PointToValue>
    {
        public override object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var t = (TextBlock)values[0];
            var point = (Point) values[1];
            var pos = (TextValuePosition)values[2];
            var value = (double)values[3];
            var formatstring = (string)values[4];
            var x = t.FontSize;
            var textWidth = t.Width;
            var top = point.Y;
            var left = point.X;
            if (pos == TextValuePosition.Outer)
            {
                if (value > 0.0)
                {
                    top -= textWidth / 2;
                    left -= textWidth;
                }
            }

            t.SetValue(Canvas.TopProperty, top);
            t.SetValue(Canvas.LeftProperty, left);
            string format = formatstring;

            var r = (RangeControl)values[5];
            if (r != null)
            {
                var g = Helpers.VisualTree.FindVisualParent<GaugeControl>(r);
                // brace yourself for gargantuan if!
                if (g != null 
                    && g.SelectedMode == GaugeModeEnum.Manual 
                    && g.IsSPTrackingEnabled == true
                    && r.Name == "ProcessValue")
                {
                    r.UpdateNeedleAndValuePointObject(value, false);
                }
            }
            return value.ToString(format);
        }

        public override object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
