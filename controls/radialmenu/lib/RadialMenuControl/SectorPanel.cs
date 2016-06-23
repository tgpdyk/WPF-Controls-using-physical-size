using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RadialMenuControl
{
    public class SectorPanel : StackPanel
    {
        /// <summary>
        /// 
        /// </summary>
        internal static DependencyProperty CenterPointProperty =
                  DependencyProperty.Register("CenterPoint",
                  typeof(Point),
                  typeof(SectorPanel),
                  new PropertyMetadata(null));

        public Point CenterPoint
        {
            get { return (Point)GetValue(CenterPointProperty); }
            set { SetValue(CenterPointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty RadiusProperty =
        DependencyProperty.Register("Radius",
                                    typeof(double),
                                    typeof(SectorPanel),
                                    new FrameworkPropertyMetadata(0d, 
                                        FrameworkPropertyMetadataOptions.AffectsMeasure));
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty Angle1Property =
             DependencyProperty.Register("Angle1",
             typeof(double),
             typeof(SectorPanel),
             new PropertyMetadata(0d));

        public double Angle1
        {
            get { return (double)GetValue(Angle1Property); }
            set { SetValue(Angle1Property, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty Angle2Property =
            DependencyProperty.Register("Angle2",
            typeof(double),
            typeof(SectorPanel),
            new PropertyMetadata(0d));

        public double Angle2
        {
            get { return (double)GetValue(Angle2Property); }
            set { SetValue(Angle2Property, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SubMenuThicknessProperty =
                                       DependencyProperty.Register("SubMenuThickness",
                                       typeof(double),
                                       typeof(SectorPanel),
                                       new FrameworkPropertyMetadata(20.0, 
                                       FrameworkPropertyMetadataOptions.AffectsMeasure));
        public double SubMenuThickness
        {
            get { return (double)GetValue(SubMenuThicknessProperty); }
            set { SetValue(SubMenuThicknessProperty, value); }
        }
        
        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            var rmItem = Helpers.VisualTree.FindVisualParent<RadialMenuItem>(this);
            if (rmItem != null)
            {
                Angle1 = rmItem.AngleStartPoint;
                Angle2 = rmItem.AngleEndPoint;
            }

            Size size = new Size(Radius, Radius);
            foreach (UIElement item in Children)
            {
                item.Measure(size);
            }
            return size;
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
        {
            foreach (UIElement item in Children)
            {
                var center = new Point(Radius, Radius);
                item.Arrange(new Rect(new Point(0,0), item.DesiredSize));
            }
            return finalSize;
        }
    }
}
