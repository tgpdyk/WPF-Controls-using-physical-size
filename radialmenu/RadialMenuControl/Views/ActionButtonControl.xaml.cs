using System.Windows;
using System.Windows.Controls;
using RadialMenuControl.Common;

namespace RadialMenuControl.Views
{
    /// <summary>
    /// Interaction logic for ButtonImageControl.xaml
    /// </summary>
    public partial class ActionButtonControl : SectorUserControl
    {
        #region _DP

        // Bad
        //public static readonly DependencyProperty ButtonImageProperty =
        //   DependencyProperty.Register("ButtonImage",
        //   typeof(Image),
        //   typeof(ActionButtonControl),
        //   new FrameworkPropertyMetadata(null, OnImageChanged));

        //public Image ButtonImage
        //{
        //    get { return (Image)GetValue(ButtonImageProperty); }
        //    set { SetValue(ButtonImageProperty, value); }
        //}

        //private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var btnCtrl = d as ActionButtonControl;
        //    if (btnCtrl != null)
        //    {
        //        Image img = (Image)e.NewValue;
        //        if (img != null)
        //        {
        //            btnCtrl.SetValue(ButtonImageSourcePropertyKey, img.Source);
        //        }
        //    }
        //}

        /*********************************************/
        //internal static DependencyPropertyKey ButtonImageSourcePropertyKey =
        //    DependencyProperty.RegisterReadOnly("ButtonImageSource",
        //    typeof(ImageSource),
        //    typeof(ActionButtonControl),
        //    new PropertyMetadata(null));

        //public static readonly DependencyProperty ButtonImageSourceProperty = ButtonImageSourcePropertyKey.DependencyProperty;

        //public ImageSource ButtonImageSource
        //{
        //    get { return (ImageSource)GetValue(ButtonImageSourceProperty); }
        //    protected set { SetValue(ButtonImageSourceProperty, value); }
        //}

        public static readonly DependencyProperty ContentPointProperty =
                          DependencyProperty.Register("ContentPoint",
                          typeof(Point),
                          typeof(ActionButtonControl),
                          new PropertyMetadata(null));
        public Point ContentPoint
        {
            get { return (Point)GetValue(ContentPointProperty); }
            set { SetValue(ContentPointProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
           DependencyProperty.Register("Label",
           typeof(string),
           typeof(ActionButtonControl),
           new FrameworkPropertyMetadata(null));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        // so bad!!!!
        public static readonly DependencyProperty Label2Property =
          DependencyProperty.Register("Label2",
          typeof(string),
          typeof(ActionButtonControl),
          new FrameworkPropertyMetadata(null));

        public string Label2
        {
            get { return (string)GetValue(Label2Property); }
            set { SetValue(Label2Property, value); }
        }

        #endregion

        public ActionButtonControl()
        {
            InitializeComponent();
        }

        public override void CalculatePoints()
        {
            base.CalculatePoints();

            var anglePos = Angle2 - Angle1;
            double radius = this.Radius;
            /*
                   HARD coded ahead!!!!
             */
            Grid grid = (Grid)ActionBtn.Template.FindName("theGrid", ActionBtn);
            if (grid != null)
            {
                if (Angle1 < 360)
                {
                    anglePos = Angle1 + ((anglePos - (grid.Width / 2) - 2));
                    radius -= grid.Width;
                }
                else
                {
                    anglePos = Angle1 + (grid.Width/2);// + 10;
                    radius -= grid.Width;
                }

                if (Label == "Interlocks")
                {
                    anglePos -= 18;
                    radius -= 25;
                }
                if (Label == "Details")
                {
                    anglePos += 1;
                    radius += 15;
                  }
            }
           

            if (anglePos > 0d)
            {
                if (Label == "Details")
                {
                    ActionBtn.Opacity = 1;
                }

                var panel = Helpers.VisualTree.FindVisualParent<SectorPanel>(this);
                if (panel != null)
                {
                    radius -= panel.SubMenuThickness;
                }
             
                var btnPoint = Helpers.GeometryHelper.CalculatePoint(CenterPoint, anglePos, radius);
                ContentPoint = btnPoint;
            }
        }
    }
}
