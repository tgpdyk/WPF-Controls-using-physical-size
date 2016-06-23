using System.Linq;
using System.Windows;
using System.Windows.Media;
using RadialMenuControl.Common;


namespace RadialMenuControl.Views
{
    /// <summary>
    /// Interaction logic for ArcImageButton.xaml
    /// 
    /// 
    /// Note: This can be abstracted with ArcButton... 
    /// </summary>
    public partial class ArcImageButton : SectorUserControl
    {
#region _DP
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", 
            typeof(Point),
            typeof(ArcImageButton),
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
           typeof(ArcImageButton),
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
            typeof(ArcImageButton),
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
            typeof(ArcImageButton),
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
            typeof(ArcImageButton),
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
            typeof(ArcImageButton),
            new PropertyMetadata(Size.Empty));
        public Size Arc2Size
        {
            get { return (Size)GetValue(Arc2SizeProperty); }
            set { SetValue(Arc2SizeProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ContentPointProperty =
                            DependencyProperty.Register("ContentPoint",
                            typeof(Point),
                            typeof(ArcImageButton),
                            new PropertyMetadata(null));
        public Point ContentPoint
        {
            get { return (Point)GetValue(ContentPointProperty); }
            set { SetValue(ContentPointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ModeProperty =
               DependencyProperty.Register("Mode",
               typeof(GaugeModeEnum),
               typeof(ArcImageButton),
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
              typeof(ArcImageButton),
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
              typeof(ArcImageButton),
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
              typeof(ArcImageButton),
              new PropertyMetadata(0d));

        public double LabelY
        {
            get { return (double)GetValue(LabelYProperty); }
            set { SetValue(LabelYProperty, value); }
        }

#endregion

        /// <summary>
        /// 
        /// </summary>
        public ArcImageButton()
        {
            InitializeComponent();
        }

        public override void CalculatePoints()
        {
            int itmIndx = 0;
            int itmCount = 0;
            SectorPanel sectorPanel = null;
            var rmItemParent = Helpers.VisualTree.FindVisualParent<RadialMenuItem>(this);
            if (rmItemParent != null)
            {
                sectorPanel = Helpers.VisualTree.FindVisualParent<SectorPanel>(rmItemParent);
                var arcImgLst = rmItemParent.Items.OfType<ArcImageButton>().ToList();
                itmCount = arcImgLst.Count();
                if (itmCount > 0)
                {
                    itmIndx = arcImgLst.IndexOf(this);
                }
            }
            
            if (itmIndx < 0) return; // Can't find the container where this item belongs..

            if (sectorPanel == null) return; // No panel to get the angles...

            var rmSector = Helpers.VisualTree.FindAncestor<RadialMenuItem>(rmItemParent);
            if (rmSector != null)
            {
                var rm = Helpers.VisualTree.FindVisualParent<RadialMenu>(sectorPanel);

                var margin = 2; // could be a property or UIElement's actual margin
                var width = rm.SubMenuContainerThickness/2.0;
                var outerRadius = (rm.Radius + width) - margin;
                var innerRadius = outerRadius - width;

                var itemAngle = (rmSector.AngleShare / itmCount) - margin; 

                var angleStart = sectorPanel.Angle1 + (itemAngle*itmIndx) + margin;
                var angleEnd = angleStart + itemAngle;

                Point arcP1 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angleStart, outerRadius);
                Point arcP2 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angleEnd, outerRadius);
                Point arcP3 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angleStart, innerRadius);
                Point arcP4 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angleEnd, innerRadius);

                StartPoint = arcP1;
                Arc1Point = arcP2;
                LinePoint = arcP4;
                Arc2Point = arcP3;

                Arc1Size = new Size(outerRadius, outerRadius);
                Arc2Size = new Size(innerRadius, innerRadius);

                ContentPoint = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angleStart, innerRadius + 30);
            }

        }
         
         private void ArcBtnItem_Click(object sender, RoutedEventArgs e)
         {
             MessageBox.Show("SubMenu click", this.Label, MessageBoxButton.OK, MessageBoxImage.Information);
         }
    }
}
