using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace RadialMenuControl.Common
{
   
    internal abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter
    where T : class, new()
    {
        private static T _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }

        #region IValueConverter Members
        public abstract object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);
        #endregion
    }

    internal abstract class MultiBindingConverterMarkupExtension<T> : MarkupExtension, IMultiValueConverter
    where T : class, new()
    {
        private static T _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }

        #region IValueConverter Members
        public abstract object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture);
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture);
        #endregion
    }
}
