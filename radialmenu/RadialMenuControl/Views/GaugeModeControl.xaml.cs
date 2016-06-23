using System.Windows;
using RadialMenuControl.Common;
using RadialMenuControl.Helpers;
using RadialMenuControl.Views.Gauge;

namespace RadialMenuControl.Views
{
    /// <summary>
    /// Interaction logic for GaugeModeControl.xaml
    /// </summary>
    public partial class GaugeModeControl : SectorUserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedModeProperty =
                 DependencyProperty.Register("SelectedMode",
                 typeof(GaugeModeEnum),
                 typeof(GaugeModeControl),
                 new PropertyMetadata(GaugeModeEnum.Auto, OnChangeMode));

        public GaugeModeEnum SelectedMode
        {
            get { return (GaugeModeEnum)GetValue(SelectedModeProperty); }
            set { SetValue(SelectedModeProperty, value); }
        }

        private static void OnChangeMode(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GaugeModeEnum mode = (GaugeModeEnum)e.NewValue;
            var gaugeModeControl = dependencyObject as GaugeModeControl;
            gaugeModeControl?.SetMode(mode);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GaugeProperty =
            DependencyProperty.Register("Gauge",
            typeof(GaugeControl),
            typeof(GaugeModeControl),
            new PropertyMetadata(null));

        public GaugeControl Gauge
        {
            get { return (GaugeControl)GetValue(GaugeProperty); }
            set { SetValue(GaugeProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public GaugeModeControl()
        {
            InitializeComponent();
            SetMode(SelectedMode);
        }

        private void SetMode(GaugeModeEnum mode)
        {
            foreach (var item in Container.Children)
            {
                var arcBtn = item as ArcButton;
                if (arcBtn != null)
                {
                    if (arcBtn.Mode == mode)
                    {
                        arcBtn.IsSelected = true;
                    }
                    else
                    {
                        arcBtn.IsSelected = false;
                    }
                }
            }
            if (Gauge != null)
            {
                Gauge.SelectedMode = mode;
            }
         }

        private void GaugeModeControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in Container.Children)
            {
                var arcBtn = item as ArcButton;
                if (arcBtn != null)
                {
                    // Check if descendant to an immediate SectorPanel..
                    var panel = VisualTree.FindVisualParent<SectorPanel>(this);
                    // If not, check the property.. There must be a binding to any instance of SectorPanel set.
                    if (panel != null)
                    {
                        arcBtn.SectorPanel = panel;
                    }
                    switch (arcBtn.Mode)
                    {
                        case GaugeModeEnum.Cascade:
                            arcBtn.Label = "T1";
                            break;
                        case GaugeModeEnum.Manual:
                            arcBtn.Label = "T2";
                            break;
                        default:
                            arcBtn.Label = "T3";
                            break;
                    }
                }
            }
        }
    }
}
