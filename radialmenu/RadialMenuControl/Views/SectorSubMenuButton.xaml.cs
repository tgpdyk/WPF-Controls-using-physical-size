using System.Linq;
using System.Windows;
using System.Windows.Data;
using RadialMenuControl.Common;

namespace RadialMenuControl.Views
{
    /// <summary>
    /// Interaction logic for SectorSubMenuButton.xaml
    /// </summary>
    internal partial class SectorSubMenuButton : SectorUserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(SectorSubMenuButton),
            new PropertyMetadata(new Point(0,0)));

        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty Arc1PointProperty =
           DependencyProperty.Register("Arc1Point", typeof(Point), typeof(SectorSubMenuButton),
           new PropertyMetadata(new Point(0, 0)));

        public Point Arc1Point
        {
            get { return (Point)GetValue(Arc1PointProperty); }
            set { SetValue(Arc1PointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LinePointProperty =
            DependencyProperty.Register("LinePoint", typeof(Point), typeof(SectorSubMenuButton),
            new PropertyMetadata(new Point(0, 0)));

        public Point LinePoint
        {
            get { return (Point)GetValue(LinePointProperty); }
            set { SetValue(LinePointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty Arc2PointProperty =
            DependencyProperty.Register("Arc2Point", typeof(Point), typeof(SectorSubMenuButton),
            new PropertyMetadata(new Point(0, 0)));

        public Point Arc2Point
        {
            get { return (Point)GetValue(Arc2PointProperty); }
            set { SetValue(Arc2PointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty Arc1SizeProperty =
            DependencyProperty.Register("Arc1Size", typeof(Size), typeof(SectorSubMenuButton),
            new PropertyMetadata(Size.Empty));
        /// <summary>
        /// 
        /// </summary>
        public Size Arc1Size
        {
            get { return (Size)GetValue(Arc1SizeProperty); }
            set { SetValue(Arc1SizeProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty Arc2SizeProperty =
            DependencyProperty.Register("Arc2Size", typeof(Size), typeof(SectorSubMenuButton),
            new PropertyMetadata(Size.Empty));

        public Size Arc2Size
        {
            get { return (Size)GetValue(Arc2SizeProperty); }
            set { SetValue(Arc2SizeProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ArrowLine1PointProperty =
            DependencyProperty.Register("ArrowLine1Point", typeof(Point), typeof(SectorSubMenuButton),
            new PropertyMetadata(new Point(0, 0)));

        public Point ArrowLine1Point
        {
            get { return (Point)GetValue(ArrowLine1PointProperty); }
            set { SetValue(ArrowLine1PointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ArrowLine2PointProperty =
            DependencyProperty.Register("ArrowLine2Point", typeof(Point), typeof(SectorSubMenuButton),
            new PropertyMetadata(new Point(0, 0)));

        public Point ArrowLine2Point
        {
            get { return (Point)GetValue(ArrowLine2PointProperty); }
            set { SetValue(ArrowLine2PointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ArrowLine3PointProperty =
            DependencyProperty.Register("ArrowLine3Point", typeof(Point), typeof(SectorSubMenuButton),
            new PropertyMetadata(new Point(0, 0)));

        public Point ArrowLine3Point
        {
            get { return (Point)GetValue(ArrowLine3PointProperty); }
            set { SetValue(ArrowLine3PointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SubMenuStateProperty =
            DependencyProperty.Register("SubMenuState", typeof(SubMenuBtnCtrlState), typeof(SectorSubMenuButton),
            new PropertyMetadata(SubMenuBtnCtrlState.SubmenuClosedWithNoItems));

        public SubMenuBtnCtrlState SubMenuState
        {
            get { return (SubMenuBtnCtrlState)GetValue(SubMenuStateProperty); }
            set { SetValue(SubMenuStateProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ThicknessProperty =
          DependencyProperty.Register("Thickness",
                                      typeof(double),
                                      typeof(SectorSubMenuButton),
                                      new PropertyMetadata(0.00, OnThicknessChanged));
        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        private static void OnThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            var btn = d as SectorSubMenuButton;
            btn?.SetCurrentValue(HeightProperty, 18.00); // 18 is just a good number, this should be a dynamic value
        }

        public SectorSubMenuButton()
        {
            InitializeComponent();

        }

        double CollapsedRadius { get; set; }

        public override void CalculatePoints()
        {
            var outerRadius = Radius;
            var innerRadius = outerRadius - Height;

            double subMenuOffset = 0d;
            if (SubMenuState == SubMenuBtnCtrlState.SubmenuOpened)
            {
                var aw = Helpers.VisualTree.FindVisualParent<RadialMenu>(this);
                if (aw != null)
                {
                    subMenuOffset = (aw.TotalDuiDiameter - aw.DuiDiameter) / 2.0;
                    outerRadius += subMenuOffset;
                    CollapsedRadius = outerRadius;
                    innerRadius = outerRadius - (Height + subMenuOffset);
                }
            }
            var angleStart = Angle1 + 0.5; //0.5 <= Refactor!
            var angleEnd = Angle2;

            Point arcP1 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angleStart, outerRadius);
            Point arcP2 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angleEnd, outerRadius);
            Point arcP3 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angleStart, innerRadius);
            Point arcP4 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angleEnd, innerRadius);

            SetValue(Point1PropertyKey, arcP1);

            Arc1Point = arcP2;
            LinePoint = arcP4;
            Arc2Point = arcP3;

            if (outerRadius > 0d && innerRadius > 0d)
            {
                Arc1Size = new Size(outerRadius, outerRadius);
                Arc2Size = new Size(innerRadius, innerRadius);

            }
            else {
                return;
            }
            /*
            * *************** ArrowHead ************************
            * 
            *                       *p1
            *                    *     *
            *                 *           *
            *               *p3  * * * * *  *p2
            */

            var arrowRadius = (outerRadius - RadialMenu.ArrowRadius) - subMenuOffset;
            var centerHalf = angleStart + ((angleEnd - angleStart) / 2);
            var arrowHeight = arrowRadius + RadialMenu.ArrowRadius / 2.00 + 3;

            double tipRadius = arrowHeight;
            double baseRadius = arrowHeight - 7;

            Point lineP1 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, centerHalf, tipRadius);
            Point lineP2 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, centerHalf - 3, baseRadius);
            Point lineP3 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, centerHalf + 3, baseRadius);

            if (SubMenuState == SubMenuBtnCtrlState.SubmenuOpened)
            {
                lineP1 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, centerHalf, baseRadius - 2);
                lineP2 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, centerHalf - 4, tipRadius);
                lineP3 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, centerHalf + 4, tipRadius);
            }
            ArrowLine1Point = lineP1;
            ArrowLine2Point = lineP2;
            ArrowLine3Point = lineP3;
        }


        private void SubMenuBtn_Click(object sender, RoutedEventArgs e)
        {

            if (SubMenuState != SubMenuBtnCtrlState.SubmenuClosedWithNoItems)
            {
                if (SubMenuState == SubMenuBtnCtrlState.SubMenuClosedWithItems)
                {
                    SubMenuState = SubMenuBtnCtrlState.SubmenuOpened;
                    ArrowDownBtn.Visibility = Visibility.Visible;
                }

                // Should be Invalidating the visual should do the trick and the most appropriate.
                // In the meantime, this should work.
                CalculatePoints();
            }
        }

        public void Button_ArrowDown_Click(object sender, RoutedEventArgs e)
        {
            if (SubMenuState == SubMenuBtnCtrlState.SubmenuOpened)
            {
                SubMenuState = SubMenuBtnCtrlState.SubMenuClosedWithItems;
                ArrowDownBtn.Visibility = Visibility.Hidden;
                CalculatePoints();
            }
        }

        private void SubMenuBtnCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            var panel = Helpers.VisualTree.FindVisualParent<SectorPanel>(this);
            if (panel != null)
            {
                var item = Helpers.VisualTree.FindVisualParent<RadialMenuItem>(panel);
                bool hasItems = item.Items.OfType<RadialMenuItem>().ToList().Any();
                if (hasItems)
                {
                    SetCurrentValue(SubMenuStateProperty, SubMenuBtnCtrlState.SubMenuClosedWithItems);
                }
                SetSubmenuItemsVisibility();
            }
        }

        void SetSubmenuItemsVisibility()
        {
            var panel = Helpers.VisualTree.FindVisualParent<SectorPanel>(this);
            if (panel != null)
            {
                var item = Helpers.VisualTree.FindVisualParent<RadialMenuItem>(panel);
                var awItemLst = item.Items.OfType<RadialMenuItem>().ToList();
                if (awItemLst.Count <= 0) return;
                var subItem = awItemLst[0];
                if (subItem != null)
                {
                    foreach (var itm in subItem.Items)
                    {
                        if (itm is ArcImageButton)
                        {
                            var bindingVisibility = new Binding
                            {
                                Source = ArrowDownBtn,
                                Path = new PropertyPath("Visibility")
                            };
                            (itm as ArcImageButton).SetBinding(VisibilityProperty, bindingVisibility);
                        }
                    }
                }
            }
        }
    }
}
