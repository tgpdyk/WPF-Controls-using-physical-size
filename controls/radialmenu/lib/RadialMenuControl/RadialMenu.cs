using System;
using System.Linq;

namespace RadialMenuControl
{
    using Helpers;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    
    public class RadialMenu : ItemsControl
    {
        /// <summary>
        ///  the Gap and the diameter of core..
        /// </summary>
        public static readonly double GapFromSubmenu = 22.00;
        public static readonly double ArrowRadius = 20.00;

        const double DefaultDiameter = 300;
        const double DefaultSubMenuThickness = 150;
        #region _DP
        /// <summary>
        /// 
        /// </summary>
        internal static DependencyPropertyKey DuiDiameterPropertyKey =
                                     DependencyProperty.RegisterReadOnly("DuiDiameter",
                                     typeof(double),
                                     typeof(RadialMenu),
                                     new PropertyMetadata(null));

        public static readonly DependencyProperty DuiDiameterProperty = DuiDiameterPropertyKey.DependencyProperty;

        public double DuiDiameter
        {
            get { return (double)GetValue(DuiDiameterProperty); }
            protected set { SetValue(DuiDiameterProperty, value); }
        }

        /// <summary>
        ///  Fixed Width in inches
        /// </summary>
        public static DependencyProperty DiameterProperty =
           DependencyProperty.Register("Diameter",
                                       typeof(double),
                                       typeof(RadialMenu),
                                       new FrameworkPropertyMetadata(DefaultDiameter, FrameworkPropertyMetadataOptions.AffectsRender, OnDiameterChanged));
        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        private static void OnDiameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (double)e.NewValue;
            if (value > 0d)
            {
                var rm = d as RadialMenu;
                rm?.SetValuesFromDiameter(value);
            }
        }
        /// <summary>
        /// Not the total radius!
        /// </summary>
        internal static DependencyPropertyKey RadiusPropertyKey =
                                      DependencyProperty.RegisterReadOnly("Radius",
                                      typeof(double),
                                      typeof(RadialMenu),
                                      new PropertyMetadata(null));

        public static readonly DependencyProperty RadiusProperty = RadiusPropertyKey.DependencyProperty;

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            protected set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        internal static DependencyPropertyKey CenterPointPropertyKey =
                                      DependencyProperty.RegisterReadOnly("CenterPoint",
                                      typeof(Point),
                                      typeof(RadialMenu),
                                      new PropertyMetadata(null));

        public static readonly DependencyProperty CenterPointProperty = CenterPointPropertyKey.DependencyProperty;

        public Point CenterPoint
        {
            get { return (Point)GetValue(CenterPointProperty); }
            protected set { SetValue(CenterPointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        internal static DependencyPropertyKey TotalDuiDiameterPropertyKey =
                                  DependencyProperty.RegisterReadOnly("TotalDuiDiameter",
                                  typeof(double),
                                  typeof(RadialMenu),
                                  new PropertyMetadata(null));

        public static readonly DependencyProperty TotalDuiDiameterProperty = TotalDuiDiameterPropertyKey.DependencyProperty;

        public double TotalDuiDiameter
        {
            get { return (double)GetValue(TotalDuiDiameterProperty); }
            protected set { SetValue(TotalDuiDiameterProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SubMenuThicknessProperty =
                                       DependencyProperty.Register("SubMenuThickness",
                                       typeof(double),
                                       typeof(RadialMenu),
                                       new FrameworkPropertyMetadata(15.00, FrameworkPropertyMetadataOptions.AffectsRender, OnSubmenuThicknessChanged));
        public double SubMenuThickness
        {
            get { return (double)GetValue(SubMenuThicknessProperty); }
            set { SetValue(SubMenuThicknessProperty, value); }
        }

        private static void OnSubmenuThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RadialMenu rm = d as RadialMenu;
            if (rm != null)
            {
                rm.IsJustInvalidateSectorPanel = true;
                rm.InvalidateMeasure();
                rm.InvalidateArrange();
                rm.InvalidateVisual();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SubMenuContainerThicknessProperty =
                                     DependencyProperty.Register("SubMenuContainerThickness",
                                     typeof(double),
                                     typeof(RadialMenu),
                                     new FrameworkPropertyMetadata(0.2, FrameworkPropertyMetadataOptions.AffectsRender));
        public double SubMenuContainerThickness
        {
            get { return (double)GetValue(SubMenuContainerThicknessProperty); }
            set { SetValue(SubMenuContainerThicknessProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StartingPointProperty =
            DependencyProperty.Register("StartingPoint", typeof(int), typeof(RadialMenu),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender),
            IsValidValid);

        public int StartingPoint
        {
            get { return (int)GetValue(StartingPointProperty); }
            set { SetValue(StartingPointProperty, value); }
        }
        // attempt to limit the angle to 360
        private static bool IsValidValid(object value)
        {
            int val = (int)value;
            return GeometryHelper.IsDegreeValueValid(val);
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
             DependencyProperty.Register("Label",
             typeof(string),
             typeof(RadialMenu),
             new FrameworkPropertyMetadata(null));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        #endregion

        public RadialMenu()
        {
            IsJustInvalidateSectorPanel = false;
            Background = Brushes.Red;
            this.Resources = ControlResources.Instance.Resource;

            Unloaded += RadialMenu_Unloaded;
        }

        private void RadialMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            // Make sure opened submenu are closed
            foreach (UIElement child in Items)
            {
                var item = VisualTree.FindVisualParent<RadialMenuItem>(child);
                var subSector = item.Items.OfType<Views.SectorSubMenuButton>().ToList().First();
                if (subSector?.SubMenuState == SubMenuBtnCtrlState.SubmenuOpened)
                {
                    // This should not be the way, so ugly! Refactor
                    subSector.Button_ArrowDown_Click(null, null);
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            double size = Math.Abs(Diameter) < 0D ? DefaultDiameter : Diameter;

            SetValuesFromDiameter(size);
        }

        bool IsJustInvalidateSectorPanel { get; set; }

        protected override Size MeasureOverride(Size constraint)
        {
            double currentAngle = (double) StartingPoint;
            if (Math.Abs(currentAngle) < 0) return Size.Empty;
            double totalAngleShare = 0d;

            // Normalization of a circle
            while (currentAngle < 0.0) currentAngle += GeometryHelper.FullCircle;
            while (currentAngle > 360.0) currentAngle -= GeometryHelper.FullCircle;

            var diameter = new Size(this.DuiDiameter, this.DuiDiameter);
           
            foreach (UIElement child in Items)
            {
                if (child is RadialMenuItem)
                {
                    var item = (child as RadialMenuItem);
                    // Sweeping Clockwise, so we need to adjust the angle.
                    item.AngleStartPoint = currentAngle;

                    var totalAngle = totalAngleShare + item.AngleShare;
                    // Make sure it won't exceed to 360...
                    if (totalAngle > 360)
                    {
                        item.AngleShare = 360.0 - totalAngleShare;
                    }
                    item.AngleEndPoint = item.AngleStartPoint + item.AngleShare;
                    currentAngle = item.AngleEndPoint;
                    totalAngleShare += item.AngleShare;
                }
                child.Measure(diameter);
            }
            return base.ArrangeOverride(diameter);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Items)
            {
                child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                var item = VisualTree.FindVisualParent<RadialMenuItem>(child);
                var hasSector = item.Items.OfType<Views.SectorSubMenuButton>().ToList().Any();
                if (hasSector) continue;
                var radialMenuItem = child as RadialMenuItem;
                radialMenuItem?.Items.Insert(0, new Views.SectorSubMenuButton());
            }
            return base.ArrangeOverride(finalSize);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is RadialMenuItem);
        }
        
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RadialMenuItem();
        }

        protected override void OnRender(DrawingContext dc)
        {
            // main ellipse
            dc.DrawEllipse(Brushes.White, new Pen(Brushes.Transparent, 0.3), CenterPoint, DuiDiameter / 2.0, DuiDiameter / 2.0);
            // core label ellipse
            var brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5B9BD5"));
            dc.DrawEllipse(Brushes.White, new Pen(brush, 1.5), CenterPoint, GapFromSubmenu, GapFromSubmenu);
            // core label 
            if (Label != null &&  !Label.Equals(""))
            {
                var text = new FormattedText(this.Label,
                              CultureInfo.CurrentCulture,
                              FlowDirection.LeftToRight,
                              new Typeface(this.FontFamily, this.FontStyle, FontWeights.Normal, this.FontStretch),
                              10,
                              this.Foreground);
                text.MaxTextWidth = 40;
                var txtPt = new Point(CenterPoint.X - text.Width / 2.0, CenterPoint.Y - text.Height / 2.0);
                dc.DrawText(text, txtPt);
            }

            var cMenu = VisualTree.FindVisualParent<ContextMenu>(this);
            
            if (cMenu != null)
            {
                var offset = DuiDiameter / 2.00;
                cMenu.HorizontalOffset = offset *-1;
            }
        }

        public void SetValuesFromDiameter(double value)
        {
            SetCurrentValue(WidthProperty, value);
            SetValue(RadialMenu.DuiDiameterPropertyKey, Width);
            var submenuContainerThickness = Math.Abs(SubMenuContainerThickness) < 0
                ? DefaultSubMenuThickness
                : SubMenuContainerThickness;
            var totalSize = Width + submenuContainerThickness;
            SetCurrentValue(WidthProperty, totalSize);
            SetCurrentValue(HeightProperty, totalSize);
            SetValue(TotalDuiDiameterPropertyKey, totalSize);
            // Radius without the submenu
            SetValue(RadialMenu.RadiusPropertyKey, DuiDiameter/2.0);
            // Centerpoint
            Point center = new Point(TotalDuiDiameter/2.0, TotalDuiDiameter/2.0);
            SetValue(CenterPointPropertyKey, center);
        }
    }
}
