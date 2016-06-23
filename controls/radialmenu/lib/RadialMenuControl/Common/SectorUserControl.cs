using System.Windows;
using System.Windows.Controls;

namespace RadialMenuControl.Common
{
    /// <summary>
    /// -> This must work hand in hand with SectorPanel.
    /// </summary>
    public class SectorUserControl : UserControl
    {
        public static DependencyProperty Angle1Property =
             DependencyProperty.Register("Angle1",
             typeof(double),
             typeof(SectorUserControl),
             new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure, OnAngle1Changed));

        public double Angle1
        {
            get { return (double)GetValue(Angle1Property); }
            set { SetValue(Angle1Property, value); }
        }

        private static void OnAngle1Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {}

        public static DependencyProperty Angle2Property =
            DependencyProperty.Register("Angle2",
            typeof(double),
            typeof(SectorUserControl),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure, OnAngle2Changed));

        public double Angle2
        {
            get { return (double)GetValue(Angle2Property); }
            set { SetValue(Angle2Property, value); }
        }

        private static void OnAngle2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {}
        
        /// <summary>
        /// Offsetting the measurement of the sector. This works like a margin.
        /// </summary>
        public static DependencyProperty AngleOffsetProperty =
            DependencyProperty.Register("AngleOffset",
            typeof(double),
            typeof(SectorUserControl),
            new FrameworkPropertyMetadata((0d), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double AngleOffset
        {
            get { return (double)GetValue(AngleOffsetProperty); }
            set { SetValue(AngleOffsetProperty, value); }
        }
     
        /// <summary>
        /// Offsetting the measurement of the sector. This works like a margin.
        /// </summary>
        public static DependencyProperty RadiusOffsetProperty =
            DependencyProperty.Register("RadiusOffset",
            typeof(double),
            typeof(SectorUserControl),
            new FrameworkPropertyMetadata((0d), FrameworkPropertyMetadataOptions.AffectsRender));

        public double RadiusOffset
        {
            get { return (double)GetValue(RadiusOffsetProperty); }
            set { SetValue(RadiusOffsetProperty, value); }
        }
        
        internal static DependencyPropertyKey Point1PropertyKey =
            DependencyProperty.RegisterReadOnly("Point1",
            typeof(Point),
            typeof(SectorUserControl),
            new PropertyMetadata(new Point(0, 0)));

        public static readonly DependencyProperty Point1Property = Point1PropertyKey.DependencyProperty;

        public Point Point1
        {
            get { return (Point)GetValue(Point1Property); }
            protected set { SetValue(Point1Property, value); }
        }
        
        internal static DependencyPropertyKey Point2PropertyKey =
            DependencyProperty.RegisterReadOnly("Point2",
            typeof(Point),
            typeof(SectorUserControl),
            new PropertyMetadata(new Point(0, 0)));

        public static readonly DependencyProperty Point2Property = Point2PropertyKey.DependencyProperty;

        public Point Point2
        {
            get { return (Point)GetValue(Point2Property); }
            protected set { SetValue(Point2Property, value); }
        }

        internal static DependencyProperty CenterPointProperty =
                  DependencyProperty.Register("CenterPoint",
                  typeof(Point),
                  typeof(SectorUserControl),
                  new PropertyMetadata(new Point(0, 0)));

        public Point CenterPoint
        {
            get { return (Point)GetValue(CenterPointProperty); }
            set { SetValue(CenterPointProperty, value); }
        }

        
        internal static DependencyPropertyKey RadiusPropertyKey =
          DependencyProperty.RegisterReadOnly("Radius",
          typeof(double),
          typeof(SectorUserControl),
          new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty RadiusProperty = RadiusPropertyKey.DependencyProperty;

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            protected set { SetValue(RadiusProperty, value); }
        }

        
        internal static DependencyPropertyKey RotationPropertyKey =
          DependencyProperty.RegisterReadOnly("Rotation",
          typeof(double),
          typeof(SectorUserControl),
          new PropertyMetadata(0d));

        public static readonly DependencyProperty RotationProperty = RotationPropertyKey.DependencyProperty;

        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            protected set { SetValue(RotationProperty, value); }
        }

        protected double TotalAngle => (Angle2 - AngleOffset) - (Angle1 + AngleOffset);

        internal static DependencyPropertyKey IsLargeArcPropertyKey =
          DependencyProperty.RegisterReadOnly("IsLargeArc",
          typeof(bool),
          typeof(SectorUserControl),
          new PropertyMetadata(false));

        public static readonly DependencyProperty IsLargeArcProperty = IsLargeArcPropertyKey.DependencyProperty;

        public bool IsLargeArc
        {
            get { return (bool)GetValue(IsLargeArcProperty); }
            protected set { SetValue(IsLargeArcProperty, value); }
        }

        
        internal static DependencyPropertyKey WheelSizePropertyKey =
          DependencyProperty.RegisterReadOnly("WheelSize",
          typeof(Size),
          typeof(SectorUserControl),
          new PropertyMetadata(Size.Empty));

        public static readonly DependencyProperty WheelSizeProperty = WheelSizePropertyKey.DependencyProperty;

        public Size WheelSize
        {
            get { return (Size)GetValue(WheelSizeProperty); }
            protected set { SetValue(WheelSizeProperty, value); }
        }

        public virtual void CalculatePoints(double radius) 
        {
            if (radius > 0d)
            {
                SetValue(RadiusPropertyKey, radius);
                CalculatePoints();
            }
        }

        public virtual void CalculatePoints()
        {
            if (Radius < 0) return;

            var radius = Radius;

            if (RadiusOffset > 0d)
            {
                radius -= RadiusOffset;
            }

            double angle1 = Angle1;
            double angle2 = Angle2;

            if (AngleOffset > 0d)
            {
                angle1 += AngleOffset;
                angle2 -= AngleOffset;
            }

            Point p1 = Helpers.GeometryHelper.CalculatePoint(CenterPoint.X, CenterPoint.Y, angle1, radius);
            Point p2 = Helpers.GeometryHelper.CalculatePoint(CenterPoint.X, CenterPoint.Y, angle2, radius);
            
            SetValue(Point1PropertyKey, p1);
            SetValue(Point2PropertyKey, p2);

            SetValue(WheelSizePropertyKey, new Size(radius, radius));
            SetValue(RotationPropertyKey, angle2 - angle1);
            SetValue(IsLargeArcPropertyKey, angle2 - angle1 > Helpers.GeometryHelper.HalfCircle);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (double.IsInfinity(constraint.Width))
            {
                return Size.Empty;
            }

            double radius = constraint.Width / 2.0;
            var panel = Helpers.VisualTree.FindVisualParent<SectorPanel>(this);
            if (panel != null)
            {
                Angle1 = panel.Angle1;
                Angle2 = panel.Angle2;
                CenterPoint = panel.CenterPoint;
                radius = panel.Radius;
            }
            CalculatePoints(radius);
            return base.MeasureOverride(constraint);
        }
    }
}
