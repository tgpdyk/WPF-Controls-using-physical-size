using DisplayDeviceInfo.AttachedProps;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Media;

namespace DisplayDeviceInfo.Behaviors
{
    public class ResizeOnDragBehavior: Behavior<Window>
    {
        protected override void OnAttached()
        {
            AssociatedObject.LocationChanged += (sender, e) =>
            {
                var w = sender as Window;
                if (w == null) return;

                var windowHandle = new WindowInteropHelper(w).Handle;
                var screen = System.Windows.Forms.Screen.FromHandle(windowHandle);

                if (ScreenInfo.Display.CurrentScreen == null) return;

                bool newScreen = !ScreenInfo.Display.CurrentScreen.Name.Equals(screen.DeviceName);

                if (newScreen)
                {
                    ScreenInfo.Display.SetCurrentScreen(w);

                    var isXResize = ElementPhysicalSize.GetResizeWidthOnDrag(w);
                    var isYResize = ElementPhysicalSize.GetResizeHeightOnDrag(w);

                    if (isXResize == true)
                    {
                        var xValue = ElementPhysicalSize.GetStaticWidth(w);
                        ElementPhysicalSize.ToDiu(w, PropertyTypeEnum.Width, xValue);
                    }

                    if (isYResize == true)
                    {
                        var yValue = ElementPhysicalSize.GetStaticHeight(w);
                        ElementPhysicalSize.ToDiu(w, PropertyTypeEnum.Height, yValue);
                    }
                    UpdateVisualChildrenWithStaticXy(w);
                }
                w.SetCurrentValue(ElementPhysicalSize.IsNewScreenDetectedProperty, newScreen);
            };
        }
        private static void UpdateVisualChildrenWithStaticXy(DependencyObject o) 
        {
            if (o != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
                {
                    var child = VisualTreeHelper.GetChild(o, i);
                    var elementChild = child as UIElement;
                    var isXResize = ElementPhysicalSize.GetResizeWidthOnDrag(elementChild);
                    var isYResize = ElementPhysicalSize.GetResizeHeightOnDrag(elementChild);
                    var xValue = ElementPhysicalSize.GetStaticWidth(elementChild);
                    var yValue = ElementPhysicalSize.GetStaticHeight(elementChild);
                    if (isXResize)
                    {
                        ElementPhysicalSize.ToDiu(elementChild, PropertyTypeEnum.Width, xValue);
                    }
                    if (isYResize)
                    {
                        ElementPhysicalSize.ToDiu(elementChild, PropertyTypeEnum.Height, yValue);
                    }

                    if (elementChild != null && VisualTreeHelper.GetChildrenCount(elementChild) > 0)
                    {
                        UpdateVisualChildrenWithStaticXy(child);
                    }
                }
            }
        }
    }
}
