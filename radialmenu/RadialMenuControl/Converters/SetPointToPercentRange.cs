using RadialMenuControl.Common;
using RadialMenuControl.Views.Gauge;

namespace RadialMenuControl.Converters
{
    internal class SetPointToPercentRange : MultiBindingConverterMarkupExtension<SetPointToPercentRange>
    {
        public override object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double valueInPercent = (double) values[0];
            double maxInPercent = (double)values[1];
            RangeControl control = (RangeControl)values[2];

            double percentage = valueInPercent / maxInPercent;

            return percentage * control.Max;
        }

        public override object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
