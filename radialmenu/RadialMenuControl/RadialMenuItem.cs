using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RadialMenuControl
{
    public class RadialMenuItem : ItemsControl
    {

        #region _DP
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AngleShareProperty =
            DependencyProperty.Register("AngleShare", 
            typeof(double), 
            typeof(RadialMenuItem),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double AngleShare
        {
            get { return (double)GetValue(AngleShareProperty); }
            set { SetValue(AngleShareProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AngleStartPointProperty =
           DependencyProperty.Register("AngleStartPoint", typeof(double), typeof(RadialMenuItem),
           new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double AngleStartPoint
        {
            get { return (double)GetValue(AngleStartPointProperty); }
            set { SetValue(AngleStartPointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AngleEndPointProperty =
            DependencyProperty.Register("AngleEndPoint", 
            typeof(double), 
            typeof(RadialMenuItem),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double AngleEndPoint
        {
            get { return (double)GetValue(AngleEndPointProperty); }
            set { SetValue(AngleEndPointProperty, value); }
        }

        public RadialMenuItem()
        {
        }

        #endregion
        public Point CenterPoint { get; set; }
        
    }
}
