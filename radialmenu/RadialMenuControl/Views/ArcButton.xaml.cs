using System.Windows;
using System.Windows.Controls;
using RadialMenuControl.Common;
using RadialMenuControl.Helpers;

namespace RadialMenuControl.Views
{
    /// <summary>
    /// Interaction logic for ArcButton.xaml
    /// </summary>
    public partial class ArcButton : SectorUserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", 
            typeof(Point),
            typeof(ArcButton),
            new PropertyMetadata(null));
        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty Arc1PointProperty =
           DependencyProperty.Register("Arc1Point",
           typeof(Point),
           typeof(ArcButton),
           new PropertyMetadata(null));
        public Point Arc1Point
        {
            get { return (Point)GetValue(Arc1PointProperty); }
            set { SetValue(Arc1PointProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LinePointProperty =
            DependencyProperty.Register("LinePoint", 
            typeof(Point),
            typeof(ArcButton),
            new PropertyMetadata(null));
        public Point LinePoint
        {
            get { return (Point)GetValue(LinePointProperty); }
            set { SetValue(LinePointProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty Arc2PointProperty =
            DependencyProperty.Register("Arc2Point",
            typeof(Point),
            typeof(ArcButton),
            new PropertyMetadata(null));
        public Point Arc2Point
        {
            get { return (Point)GetValue(Arc2PointProperty); }
            set { SetValue(Arc2PointProperty, value); }
        }
     
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty Arc1SizeProperty =
            DependencyProperty.Register("Arc1Size", 
            typeof(Size),
            typeof(ArcButton),
            new PropertyMetadata(Size.Empty));
        public Size Arc1Size
        {
            get { return (Size)GetValue(Arc1SizeProperty); }
            set { SetValue(Arc1SizeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty Arc2SizeProperty =
            DependencyProperty.Register("Arc2Size",
            typeof(Size),
            typeof(ArcButton),
            new PropertyMetadata(Size.Empty));
        public Size Arc2Size
        {
            get { return (Size)GetValue(Arc2SizeProperty); }
            set { SetValue(Arc2SizeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SectorPanelProperty =
            DependencyProperty.Register("SectorPanel",
            typeof(SectorPanel),
            typeof(ArcButton),
            new PropertyMetadata(null));
        public SectorPanel SectorPanel
        {
            get { return (SectorPanel)GetValue(SectorPanelProperty); }
            set { SetValue(SectorPanelProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ModeProperty =
               DependencyProperty.Register("Mode",
               typeof(GaugeModeEnum),
               typeof(ArcButton),
               new PropertyMetadata(GaugeModeEnum.Auto));
        public GaugeModeEnum Mode
        {
            get { return (GaugeModeEnum)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
       /// <summary>
       /// 
       /// </summary>
        public static readonly DependencyProperty LabelProperty =
              DependencyProperty.Register("Label",
              typeof(string),
              typeof(ArcButton),
              new PropertyMetadata(""));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LabelXProperty =
              DependencyProperty.Register("LabelX",
              typeof(double),
              typeof(ArcButton),
              new PropertyMetadata(0d));

        public double LabelX
        {
            get { return (double)GetValue(LabelXProperty); }
            set { SetValue(LabelXProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LabelYProperty =
              DependencyProperty.Register("LabelY",
              typeof(double),
              typeof(ArcButton),
              new PropertyMetadata(0d));

        public double LabelY
        {
            get { return (double)GetValue(LabelYProperty); }
            set { SetValue(LabelYProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected",
            typeof(bool),
            typeof(ArcButton),
            new PropertyMetadata(false));
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        // Can be a be DP 
        private const double TopMargin = 25;
        private const double ButtonHeight = 22.00;

        public ArcButton()
        {
            InitializeComponent();
        }
        public override void CalculatePoints()
        {
             if (SectorPanel != null)
             {
                 var outerRadius = Radius;

                 var submenuBtn = VisualTree.FindVisualChild<SectorSubMenuButton>(SectorPanel);
                 if (submenuBtn != null)
                 {
                     outerRadius -= SectorPanel.SubMenuThickness;
                 }
                 else
                 {
                     outerRadius -= TopMargin;
                 }

                 var stackPanel = VisualTree.FindVisualParent<StackPanel>(this);
                 var idx = stackPanel.Children.IndexOf(this);

                 if (idx > 0)
                 {
                     var newOffset = ButtonHeight * idx;
                     outerRadius -= newOffset;
                 }
                 
                 outerRadius -= 5;

                 var innerRadius = outerRadius - ButtonHeight;
                 var item = VisualTree.FindVisualParent<RadialMenuItem>(SectorPanel);
                 
                 var angleStart = item.AngleStartPoint + AngleOffset;
                 var angleEnd = item.AngleEndPoint - AngleOffset;

                /*
                 SubMenu button 
                */
                 Point arcP1 = GeometryHelper.CalculatePoint(SectorPanel.CenterPoint, angleStart, outerRadius);
                 Point arcP2 = GeometryHelper.CalculatePoint(SectorPanel.CenterPoint, angleEnd, outerRadius);
                 Point arcP3 = GeometryHelper.CalculatePoint(SectorPanel.CenterPoint, angleStart, innerRadius);
                 Point arcP4 = GeometryHelper.CalculatePoint(SectorPanel.CenterPoint, angleEnd, innerRadius);
                 StartPoint = arcP1;
                 Arc1Point = arcP2;
                 LinePoint = arcP4;
                 Arc2Point = arcP3;
                 Arc1Size = new Size(outerRadius, outerRadius);
                 Arc2Size = new Size(innerRadius, innerRadius);
                 var centerRadius = innerRadius + ((outerRadius - innerRadius) / 2);
                 var centerAngle = angleStart + ((angleEnd - angleStart) / 2);
                 Point txtPoint = GeometryHelper.CalculatePoint(SectorPanel.CenterPoint, centerAngle, centerRadius);
                 LabelX = txtPoint.X - RadialMenu.GapFromSubmenu;
                 LabelY = txtPoint.Y - ((outerRadius - innerRadius) / 2) + 5; // Must be dynamic based on the size of sibling
             }
         }
         
         private void ArcBtnItem_Click(object sender, RoutedEventArgs e)
         {
             IsSelected = !IsSelected;
             var gaugeCtrl = VisualTree.FindVisualParent<GaugeModeControl>(this);

             if (gaugeCtrl != null)
             {
                 if (IsSelected)
                 {
                     gaugeCtrl.SetValue(GaugeModeControl.SelectedModeProperty, this.Mode);
                 }
             }
         }
    }
}
