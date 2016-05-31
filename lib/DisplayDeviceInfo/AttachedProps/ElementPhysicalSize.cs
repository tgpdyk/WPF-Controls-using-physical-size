using System.Windows;
using DisplayDeviceInfo.Interfaces;

namespace DisplayDeviceInfo.AttachedProps
{
    public enum PropertyTypeEnum
    { 
        Width,
        Height
    };
    /// <summary>
    /// Attached property to Resolution-aware WPF controls for Width and Height
    /// </summary>
    public class ElementPhysicalSize
    {

        /// <summary>
        ///  Resize when dragging to other monitor for Width
        /// </summary>
        private static readonly DependencyProperty ResizeWidthOnDragProperty =
           DependencyProperty.RegisterAttached("ResizeWidthOnDrag",
                                       typeof(bool),
                                       typeof(ElementPhysicalSize),
                                       new FrameworkPropertyMetadata(false));

        private static void SetResizeWidthOnDrag(DependencyObject d, bool value)
        {
            d.SetValue(ResizeWidthOnDragProperty, value);
        }

        public static bool GetResizeWidthOnDrag(UIElement element)
        {
            return (bool)element.GetValue(ResizeWidthOnDragProperty);
        }


        /// <summary>
        ///  Resize when dragging to other monitor for Width
        /// </summary>
        private static readonly DependencyProperty ResizeHeightOnDragProperty =
           DependencyProperty.RegisterAttached("ResizeHeightOnDrag",
                                       typeof(bool),
                                       typeof(ElementPhysicalSize),
                                       new FrameworkPropertyMetadata(false));

        public static bool GetResizeHeightOnDrag(UIElement element)
        {
            return (bool)element.GetValue(ResizeHeightOnDragProperty);
        }


        public static readonly DependencyProperty IsNewScreenDetectedProperty =
            DependencyProperty.RegisterAttached("IsNewScreenDetected",
                                    typeof(bool),
                                    typeof(ElementPhysicalSize),
                                    new FrameworkPropertyMetadata(false));

        public static void SetIsNewScreenDetected(DependencyObject d, bool value)
        {
            d.SetValue(IsNewScreenDetectedProperty, value);
        }

        public static bool GetIsNewScreenDetected(UIElement element)
        {
            return (bool)element.GetValue(IsNewScreenDetectedProperty);
        }

        /// <summary>
        ///  Fixed Width in inches
        /// </summary>
        public static DependencyProperty StaticWidthProperty =
           DependencyProperty.RegisterAttached("StaticWidth",
                                       typeof(double),
                                       typeof(ElementPhysicalSize),
                                       new FrameworkPropertyMetadata(10.00, FrameworkPropertyMetadataOptions.AffectsMeasure, OnValueWidthChanged));

        public static void SetStaticWidth(DependencyObject d, double value)
        {
            d.SetValue(StaticWidthProperty, value);
            d.SetValue(ResizeWidthOnDragProperty, true);
        }

        public static double GetStaticWidth(UIElement element)
        {
            return (double)element.GetValue(StaticWidthProperty);
        }

        private static void OnValueWidthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var value = (double) e.NewValue;
            ElementPhysicalSize.SetStaticWidth(dependencyObject, value);
            ToDiu(dependencyObject, PropertyTypeEnum.Width, value);
         }
        /// <summary>
        ///  Fixed Height in inches
        /// </summary>
        public static DependencyProperty StaticHeightProperty =
          DependencyProperty.RegisterAttached("StaticHeight",
                                      typeof(double),
                                      typeof(ElementPhysicalSize),
                                      new FrameworkPropertyMetadata(10.00, FrameworkPropertyMetadataOptions.AffectsMeasure, OnValueHeightChanged));


        public static void SetStaticHeight(DependencyObject d, double value)
        {
            d.SetValue(StaticHeightProperty, value);
            d.SetValue(ResizeHeightOnDragProperty, true);
        }

        public static double GetStaticHeight(UIElement element)
        {
            return (double)element.GetValue(StaticHeightProperty);
        }
       
        private static void OnValueHeightChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var value = (double)e.NewValue;
            ElementPhysicalSize.SetStaticHeight(dependencyObject, value);
            if (ToDiu(dependencyObject, PropertyTypeEnum.Height, value) == false)
            { 
                // Something is wrong. Possibly there is no monitor detected.
            }
        }
        /// <summary>
        /// Convert the value (in inches) to Device Independend Units
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public static bool ToDiu(DependencyObject dependencyObject, PropertyTypeEnum type, double value)
        {
            var elementToResize = dependencyObject as FrameworkElement;
            if (elementToResize != null)
            {
                double valueInInches = 0D;
                // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                if (double.TryParse(value.ToString(), out valueInInches))
                {
                    if (valueInInches > 0D)
                    {
                        Window parentWnd = null;
                        if (dependencyObject is Window)
                        {
                            parentWnd = dependencyObject as Window;
                        }
                        else
                        {
                            parentWnd = Window.GetWindow(dependencyObject);
                        }

                        if (parentWnd != null)
                        {
                            IDisplayDevice device = ScreenInfo.Display.SetCurrentScreen(parentWnd);
                            var currentScreen = ScreenInfo.Display.CurrentScreen;
                            if (device != null)
                            {
                                double convertedValue = 0D;
                                double ppi = 0D;
                                DependencyProperty prop = null;
                                // Height
                                if (type == PropertyTypeEnum.Height)
                                {
                                    ppi = device.GetYPpi((double)currentScreen.WidthPx, (double)currentScreen.HeightPx);
                                    convertedValue = valueInInches * ppi;
                                    prop = FrameworkElement.HeightProperty;
                                }
                                // Width
                                else
                                {
                                    ppi = device.GetXPpi((double)currentScreen.WidthPx, (double)currentScreen.HeightPx);
                                    convertedValue = valueInInches * ppi;
                                    prop = FrameworkElement.WidthProperty;
                                }
                                if (!double.IsInfinity(convertedValue))
                                {
                                    if (convertedValue > 0d)
                                    {
                                        elementToResize.SetCurrentValue(prop, convertedValue);
                                        return true;
                                    }
                                }
                            }
                            else {
                                System.Diagnostics.Debug.WriteLine("No selected screen/monitor");
                                //MessageBox.Show("No selected screen/monitor");
                            }
                        }
                        else {
                            //MessageBox.Show("It should be a child or descendant of a Window");
                            System.Diagnostics.Debug.WriteLine("It should be a child or descendant of a Window");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot convert value of 0");
                        System.Diagnostics.Debug.WriteLine("Cannot convert value of 0");
                    }
                }
                else
                {
                    MessageBox.Show("Value is not of double type");
                    System.Diagnostics.Debug.WriteLine("Value is not of double type");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Element has no Width or Height property");
            }
            return false;
        }

        public static bool ToDiu(DependencyObject dependencyObject, double valuePhysical, out Size converted)
        {
            converted = Size.Empty;
            var element = dependencyObject as FrameworkElement;
            if (element != null)
            { 
                double valueInInches = 0D;
                if (double.TryParse(valuePhysical.ToString(), out valueInInches))
                {
                    if (valueInInches > 0D)
                    {
                        Window parentWnd = null;
                        if (dependencyObject is Window)
                        {
                            parentWnd = dependencyObject as Window;
                        }
                        else
                        {
                            parentWnd = Window.GetWindow(dependencyObject);
                        }
                        IScreen scr = null;
                        IDisplayDevice device = null;
                        if (parentWnd == null)
                        {
                            scr = ScreenInfo.Display.CurrentScreen;
                            if (scr == null)
                            {
                                parentWnd = Application.Current.MainWindow;
                                device = ScreenInfo.Display.SetCurrentScreen(parentWnd);
                                scr = ScreenInfo.Display.CurrentScreen;
                            }
                            else
                            {
                                device = scr.Device;
                            }
                        }
                        else
                        {

                            device = ScreenInfo.Display.SetCurrentScreen(parentWnd);
                            scr = ScreenInfo.Display.CurrentScreen;

                        }

                        if (device != null)
                        {
                            var yPpi = device.GetYPpi((double)scr.WidthPx, (double)scr.HeightPx);
                            var xPpi = device.GetXPpi((double)scr.WidthPx, (double)scr.HeightPx);

                            if (yPpi > 0d && xPpi > 0d)
                            {
                                converted = new Size(xPpi * valueInInches, yPpi * valueInInches);
                                return true;
                            }
                        }
                        // error
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
