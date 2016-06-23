using RadialMenuControl.Common;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using RadialMenuControl.Helpers;

namespace RadialMenuControl.Views.Gauge
{

    /// <summary>
    /// Interaction logic for RangeControl.xaml
    /// </summary>
    public partial class RangeControl : SectorUserControl
    {

        /// <summary>
        /// 
        /// </summary>
        internal static DependencyPropertyKey NeedlePoint1PropertyKey =
                  DependencyProperty.RegisterReadOnly("NeedlePoint1",
                  typeof(Point),
                  typeof(RangeControl),
                  new PropertyMetadata(null));

        public static readonly DependencyProperty NeedlePoint1Property = NeedlePoint1PropertyKey.DependencyProperty;

        public Point NeedlePoint1
        {
            get { return (Point)GetValue(NeedlePoint1Property); }
            protected set { SetValue(NeedlePoint1Property, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal static DependencyPropertyKey NeedlePoint2PropertyKey =
                  DependencyProperty.RegisterReadOnly("NeedlePoint2",
                  typeof(Point),
                  typeof(RangeControl),
                  new PropertyMetadata(null));

        public static readonly DependencyProperty NeedlePoint2Property = NeedlePoint2PropertyKey.DependencyProperty;

        public Point NeedlePoint2
        {
            get { return (Point)GetValue(NeedlePoint2Property); }
            protected set { SetValue(NeedlePoint2Property, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal static DependencyPropertyKey NeedleTextValuePointPropertyKey =
                  DependencyProperty.RegisterReadOnly("NeedleTextValuePoint",
                  typeof(Point),
                  typeof(RangeControl),
                  new PropertyMetadata(null));

        public static readonly DependencyProperty NeedleTextValuePointProperty = NeedleTextValuePointPropertyKey.DependencyProperty;

        public Point NeedleTextValuePoint
        {
            get { return (Point)GetValue(NeedleTextValuePointProperty); }
            protected set { SetValue(NeedleTextValuePointProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TextValueSectorProperty =
            DependencyProperty.Register("TextValueSector",
            typeof(Sector),
            typeof(RangeControl),
            new PropertyMetadata(null));

        public Sector TextValueSector
        {
            get { return (Sector)GetValue(TextValueSectorProperty); }
            set { SetValue(TextValueSectorProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min",
            typeof(double),
            typeof(RangeControl),
            new PropertyMetadata(0d));
        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max",
            typeof(double),
            typeof(RangeControl),
            new PropertyMetadata(0d));

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit",
            typeof(string),
            typeof(RangeControl),
            new PropertyMetadata(""));

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CurrentRangeValueProperty =
           DependencyProperty.Register("CurrentRangeValue",
           typeof(double),
           typeof(RangeControl),
           new PropertyMetadata(0d, OnCurrentRangeValueChanged));

        public double CurrentRangeValue
        {
            get { return (double)GetValue(CurrentRangeValueProperty); }
            set { SetValue(CurrentRangeValueProperty, value); }
        }

        private static void OnCurrentRangeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeCtrl = d as RangeControl;
            if (rangeCtrl != null)
            {
                double v = (double) e.NewValue;

                if (rangeCtrl.AnimateNeedleBox == true)
                {
                    var g = VisualTree.FindVisualParent<GaugeControl>(rangeCtrl);
                    if (g != null && g.IsSPTrackingEnabled == true)
                    {
                        rangeCtrl.NeedleTextValue = v;
                    }
                }
                else
                {

                    bool isAnimationRunning = false;

                    if (rangeCtrl.RangeValueAnimation != null)
                    {
                        var state = rangeCtrl.RangeValueAnimation.GetCurrentState(rangeCtrl);
                        isAnimationRunning = state == ClockState.Active;
                    }


                    if (isAnimationRunning == false)
                    {
                        var g = VisualTree.FindVisualParent<GaugeControl>(rangeCtrl);
                        if (g != null &&  g.IsSPTrackingEnabled == true)
                        {
                            rangeCtrl.NeedleTextValue = v;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// used when needle is dragging for value updated
        /// </summary>
        public static readonly DependencyProperty NeedleTextValueProperty =
           DependencyProperty.Register("NeedleTextValue",
           typeof(double),
           typeof(RangeControl),
           new FrameworkPropertyMetadata(0d));

        public double NeedleTextValue
        {
            get { return (double)GetValue(NeedleTextValueProperty); }
            set { SetValue(NeedleTextValueProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty PreviousRangeValueProperty =
           DependencyProperty.Register("PreviousRangeValueProperty",
           typeof(double),
           typeof(RangeControl),
           new FrameworkPropertyMetadata(0d));

        public double PreviousRangeValue
        {
            get { return (double)GetValue(PreviousRangeValueProperty); }
            set { SetValue(PreviousRangeValueProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty NeedleBrushProperty =
            DependencyProperty.Register("NeedleBrush",
            typeof(Brush),
            typeof(RangeControl),
            new FrameworkPropertyMetadata(Brushes.Gray, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush NeedleBrush
        {
            get { return (Brush)GetValue(NeedleBrushProperty); }
            set { SetValue(NeedleBrushProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty NeedleVisibilityProperty =
           DependencyProperty.Register("NeedleVisibility",
           typeof(Visibility),
           typeof(RangeControl),
           new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.AffectsRender));

        public Visibility NeedleVisibility
        {
            get { return (Visibility)GetValue(NeedleVisibilityProperty); }
            set { SetValue(NeedleVisibilityProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable",
            typeof(bool),
            typeof(RangeControl),
            new PropertyMetadata(true, OnEditableValueChanged));

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        private static void OnEditableValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeCtrl = d as RangeControl;
            if (rangeCtrl != null)
            {
                bool b = (bool) e.NewValue;
                if (b == true)
                {
                    rangeCtrl.BoxSetPoint.Visibility = Visibility.Visible;
                }
                else {
                    rangeCtrl.BoxSetPoint.Visibility = Visibility.Hidden;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TextValuePositionProperty =
           DependencyProperty.Register("TextValuePosition",
           typeof(TextValuePosition),
           typeof(RangeControl),
           new PropertyMetadata(TextValuePosition.Outer));

        public TextValuePosition TextValuePosition
        {
            get { return (TextValuePosition)GetValue(TextValuePositionProperty); }
            set { SetValue(TextValuePositionProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TextValueFormatProperty =
           DependencyProperty.Register("TextValueFormat",
           typeof(string),
           typeof(RangeControl),
           new PropertyMetadata("0"));

        public string TextValueFormat
        {
            get { return (string)GetValue(TextValueFormatProperty); }
            set { SetValue(TextValueFormatProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ValuePointProperty =
            DependencyProperty.Register("ValuePoint", 
            typeof(Point), 
            typeof(RangeControl), 
            new PropertyMetadata(new Point(0,0)));

        public Point ValuePoint
        {
            get { return (Point)GetValue(ValuePointProperty); }
            set { SetValue(ValuePointProperty, value); }
        }

        private bool _isMouseRotating = false;
        private Vector _mouseDownVector;
        private double _mouseAngle;
        private double ValueAngle { get; set; }

        public Storyboard RangeValueAnimation = null;
        private Storyboard _textValueAnimation = null;
        private Storyboard _textValueAnimation2 = null;

        public bool AnimateNeedleBox { get; set; }

        public RangeControl()
        {
            InitializeComponent();
            AnimateNeedleBox = false;
        }

        public override void CalculatePoints()
        {
            var a1 = Angle1;
            var a2 = Angle2;
            var radius = Radius;

            // Refactor: Can be a const OR configurable OR DP
            double gapFromSubmenu = 22.00;
            double gapBetweenAngles = 5;

            var radiusOffset = 20 + gapFromSubmenu;

            var sectorPanel = Helpers.VisualTree.FindVisualParent<SectorPanel>(this);
            var gauge = Helpers.VisualTree.FindVisualChild<GaugeControl>(sectorPanel);
           
            int idx = gauge.RangesPanel.Children.IndexOf(this);

            // Adjust radius - to separate both range a bit with space
            if (idx > 0)
            {
                radiusOffset += (idx * PathRange.StrokeThickness) + (gapFromSubmenu / 2);
            }

            Sector rangeSector = new Sector(radius, CenterPoint, a1, a2, radiusOffset, gapBetweenAngles);

            RadiusOffset = radiusOffset;
            AngleOffset = gapBetweenAngles;

            base.CalculatePoints();

            if (TextValuePosition == TextValuePosition.Outer)
            {
                TextValueSector = new Sector(radius,CenterPoint, a1, a2, radiusOffset - 5, gapBetweenAngles);
                var textValueVm = new ValueViewModel(TextValueSector, Min, Max, CurrentRangeValue, null);
                PathTextValue.DataContext = textValueVm;    
            }
            else 
            {
                TextValueSector = new Sector(radius, CenterPoint, a1, a2, radiusOffset + 7, gapBetweenAngles);
                var textValueVm = new ValueViewModel(TextValueSector, Min, Max, CurrentRangeValue, null);
                PathTextValue.DataContext = textValueVm;
            }
            
            double topMin = rangeSector.PointStart.Y;
            double leftMin = rangeSector.PointStart.X;

            double topMax = rangeSector.PointEnd.Y;
            double leftMax = rangeSector.PointEnd.X;

            // Outer arc text
            if (idx == 0)
            {
                leftMin -= 7;
                leftMax -= 15;
                topMax -= 10;
            }
            // Inner arc text
            else if (idx == 1)
            {
                topMin -= 15;
                leftMax -= 10;
                topMax += 5;
            }
                
            // Min
            TextMin.Text = Min.ToString();
            if (TextValuePosition == TextValuePosition.Outer)
            {
                TextMin.SetValue(Canvas.TopProperty, TextValueSector.PointStart.Y - 5);
                TextMin.SetValue(Canvas.LeftProperty, TextValueSector.PointStart.X);
            }
            else
            {
                TextMin.SetValue(Canvas.TopProperty, TextValueSector.PointStart.Y - 2);
                TextMin.SetValue(Canvas.LeftProperty, TextValueSector.PointStart.X);
            }
            // Max
            TextMax.Text = $"{Max} {Unit}";
            TextMax.SetValue(Canvas.TopProperty, topMax);
            TextMax.SetValue(Canvas.LeftProperty, leftMax);
            // BAD - Refactor
            var p = new Point(BoxSetPoint.Width, BoxSetPoint.Height);
            ValueViewModel vm = new ValueViewModel(rangeSector, Min, Max, CurrentRangeValue, p);
            BoxSetPoint.DataContext = vm;
            UpdateNeedleAndValuePointObject(CurrentRangeValue);
        }

        public void UpdateNeedleAndValuePointObject(double value, bool updateValuePoint = true)
        {
            if (value > 0d)
            {
                var val = value / Max;
                var angle = Angle1 + AngleOffset + (TotalAngle * val);
                var p1 = GeometryHelper.CalculatePoint(CenterPoint, angle, Radius - RadiusOffset);
                var p2 = GeometryHelper.CalculatePoint(CenterPoint, angle, 23); // 23 is the core radius
                var textPoint = GeometryHelper.CalculatePoint(CenterPoint, angle, (Radius - RadiusOffset) / 2);
                Point p3 = new Point(textPoint.Y - BoxSetPoint.Height / 2, textPoint.X - BoxSetPoint.Width / 2);
                SetValue(NeedlePoint1PropertyKey, p1);
                SetValue(NeedlePoint2PropertyKey, p2);
                SetValue(NeedleTextValuePointPropertyKey, p3);
                if (updateValuePoint == true)
                { 
                    ValuePoint = p1; 
                }
                Debug.WriteLine("* VALUEPOINT = {0}", ValuePoint); 
                var vm = PathTextValue.DataContext as ValueViewModel;
                vm?.UpdateValue(CurrentRangeValue);
            }
        }

        public void UpdateValue(double newValue, 
            bool vmIsUpdated = false, 
            bool updateNeedle = false,
            double animateInSec = 1.5,
            double beginTime = 0.0)
        {
            double oldValue = CurrentRangeValue;

            CurrentRangeValue = newValue;
            PreviousRangeValue = oldValue;
            
            var val = CurrentRangeValue / Max;
            var angle = Angle1 + AngleOffset + (TotalAngle * val);
            
            var textValueVm = PathTextValue.DataContext as ValueViewModel;
            if (textValueVm != null)
            {
                textValueVm.UpdateValue(newValue);
            }

            if (updateNeedle == true)
            {
                UpdateNeedleAndValuePointObject(newValue);
            }
            AnimateRangeValue(newValue, oldValue, animateInSec);
        }

        
        private void BoxSetPoint_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Let the parent do the updating and call the animation of corresponding RangeControl...
                //var gauge = Helpers.VisualTree.FindVisualParent<GaugeControl>(this);

                var textboxVal = sender as TextBox;
                if (textboxVal != null)
                {
                    var newVal = textboxVal.Text;
                    double newValue = -1.00;
                    if (double.TryParse(newVal, out newValue))
                    {
                        if (newValue >= 0d)
                        {
                            ValueViewModel valVm = textboxVal.DataContext as ValueViewModel;
                            if (valVm != null)
                            {
                                Point oldPoint = valVm.ValuePoint;

                                if (!valVm.UpdateValue(newValue))
                                {
                                    textboxVal.Text = CurrentRangeValue.ToString();
                                }
                                else
                                {
                                    valVm.UpdateValue(newValue);
                                    double oldValue = CurrentRangeValue;
                                    if (oldValue != newValue) // a bit paranoid!
                                    {
                                        var gauge = Helpers.VisualTree.FindVisualParent<GaugeControl>(this);
                                        gauge?.UpdateValue(newValue, this);
                                        UpdateNeedleAndValuePointObject(newValue, false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #region _ANIMATION

        public void AnimateRangeValue(double newValue, double oldValue, double seconds = 1.5, double begin = 0.0)
        {
            if (RangeValueAnimation == null)
            {
                RangeValueAnimation = MainCanvas.FindResource("RangeValueAnimation") as Storyboard;
            }

            if (RangeValueAnimation != null)
            {
                
                // Decreasing or Increasing...
                bool hasIncreased = newValue > oldValue;

                var pNew = newValue / Max;
                var pOld = oldValue / Max;
                var angleNew = Angle1 + AngleOffset + (TotalAngle * pNew);
                var angleOld = Angle1 + AngleOffset + (TotalAngle * pOld);

                var radius = Radius - RadiusOffset;

                Point from = Helpers.GeometryHelper.CalculatePoint(CenterPoint.X, CenterPoint.Y, angleOld, radius);
                Point to = Helpers.GeometryHelper.CalculatePoint(CenterPoint.X, CenterPoint.Y, angleNew, radius);

                var direction = (hasIncreased == true) ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
                // first animation
                PointAnimationUsingPath point = RangeValueAnimation.Children[0] as PointAnimationUsingPath;

                point.Duration = new Duration(TimeSpan.FromSeconds(seconds));
                point.BeginTime = TimeSpan.FromSeconds(begin);

                PathFigure fig = point.PathGeometry.Figures[0];
                fig.StartPoint = from;
                ArcSegment segment = fig.Segments[0] as ArcSegment;
                segment.Size = WheelSize;
                segment.RotationAngle = Rotation;
                segment.Point = to;
                segment.SweepDirection = direction;

                try
                {
                    RangeValueAnimation.Begin(this, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            if (_textValueAnimation == null)
            {
                _textValueAnimation = MainCanvas.FindResource("TextValueAnimation") as Storyboard;
            }

            if (_textValueAnimation != null)
            {
                //_TextValueAnimation.Duration = new Duration(TimeSpan.FromSeconds(seconds));
                bool hasIncreased = newValue > oldValue;
                // second -- the hidden one!
                var pNew1 = newValue / Max;
                var pOld1 = oldValue / Max;
                var angleNew1 = TextValueSector.AngleStart + (TextValueSector.TotalAngle * pNew1);
                var angleOld1 = TextValueSector.AngleStart + (TextValueSector.TotalAngle * pOld1);

                var from = Helpers.GeometryHelper.CalculatePoint(TextValueSector.Center, angleOld1, TextValueSector.Radius);
                var to = Helpers.GeometryHelper.CalculatePoint(TextValueSector.Center, angleNew1, TextValueSector.Radius);

                var direction = (hasIncreased == true) ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;

                PointAnimationUsingPath point1 = _textValueAnimation.Children[0] as PointAnimationUsingPath;
                point1.Duration = new Duration(TimeSpan.FromSeconds(seconds));
                point1.BeginTime = TimeSpan.FromSeconds(begin);
                PathFigure fig1 = point1.PathGeometry.Figures[0];
                fig1.StartPoint = from;
                ArcSegment segment1 = fig1.Segments[0] as ArcSegment;
                segment1.Size = TextValueSector.Size;
                segment1.RotationAngle = TextValueSector.Rotation;
                segment1.Point = to;
                segment1.SweepDirection = direction;

                try
                {
                    _textValueAnimation.Begin(this);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (_textValueAnimation2 == null)
            {
                _textValueAnimation2 = MainCanvas.FindResource("TextValueAnimation2") as Storyboard;
            }
            
            if (_textValueAnimation2 != null)
            {
                //_TextValueAnimation2.Duration = new Duration(TimeSpan.FromSeconds(seconds));
                DoubleAnimation doubleAnima = _textValueAnimation2.Children[0] as DoubleAnimation;
                doubleAnima.Duration = new Duration(TimeSpan.FromSeconds(seconds));
                doubleAnima.BeginTime = TimeSpan.FromSeconds(begin);
                if (doubleAnima != null)
                {
                    doubleAnima.From = oldValue;
                    doubleAnima.To = newValue;
                }
               
                try
                {
                    _textValueAnimation2.Begin(this);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }
        #endregion

        private void PathSetPointNeedle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && IsEditable)
            {
                var mouseDownPoint = e.GetPosition(PathSetPointNeedle);
                _mouseDownVector = mouseDownPoint - CenterPoint;
                _mouseAngle = ValueAngle;
                e.MouseDevice.Capture(PathSetPointNeedle);
                _isMouseRotating = true;
            }
        }

        private void PathSetPointNeedle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isMouseRotating == true && IsEditable)
            {
                e.MouseDevice.Capture(null);
                _isMouseRotating = false;
                var angleStart = Angle1 + AngleOffset;
                var currentAngle = ValueAngle;
                
                var p = (currentAngle - angleStart) / TotalAngle;
                double value = p * Max;

                if ((int)value != (int)CurrentRangeValue) // casting to int is a workaround
                {
                    var gauge = VisualTree.FindVisualParent<GaugeControl>(this);
                    gauge?.UpdateValue(value, this);
                    UpdateNeedleAndValuePointObject(value, false);
                }
            }
        }

        private void PathSetPointNeedle_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseRotating == true && e.LeftButton == MouseButtonState.Pressed && IsEditable)
            {
               // Debug.WriteLine("*** rotate mouse_angle= {0}", _MouseAngle);

                Point curPos = e.GetPosition(PathSetPointNeedle);
                Vector currV = curPos - CenterPoint;
                double angle = Vector.AngleBetween(_mouseDownVector, currV) + _mouseAngle;

                //Debug.WriteLine("*** rotate angle after formula= {0}", angle);
                // cast it to int to avoid double comparison problem
                var angleAsInt = (int)angle;
                var angleEnd = Angle2 - AngleOffset;
                var angleStart = Angle1 + AngleOffset;

                if (angleAsInt > angleEnd)
                {
                    angle = angleEnd;
                }
                else if (angleAsInt < angleStart)
                {
                    angle = angleStart;
                }

                //Debug.WriteLine("*** rotate= {0}", angle);

                /*
                 * seems like this is in a different thread. there is a need for setting the props here
                 * there is a bad thing happening (lag) when we call the upate (such as UpdateValue())...
                 */
                var p1 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angle, Radius - RadiusOffset);
                var p2 = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angle, 23); // 23 is the core radius
                var textPoint = Helpers.GeometryHelper.CalculatePoint(CenterPoint, angle, (Radius - RadiusOffset) / 2);
                Point p3 = new Point(textPoint.Y -  BoxSetPoint.Height / 2, textPoint.X - BoxSetPoint.Width / 2);

                SetValue(NeedlePoint1PropertyKey, p1);
                SetValue(NeedlePoint2PropertyKey, p2);
                SetValue(NeedleTextValuePointPropertyKey, p3);

                var p = (angle - angleStart) / TotalAngle;
                double value = p * Max;
                NeedleTextValue = value;
                ValueAngle = angle;
                //Debug.WriteLine("**** mousemove! Angle={0}, p1={1}, p2={2}", angle, p1, p2);
            }
        }

        public bool _IsAnimationToOtherNeeded = true;
        
        private void RangeValue_Storyboard_Completed(object sender, EventArgs e)
        {}

        private void TextValue_Storyboard_Completed(object sender, EventArgs e)
        {   // refactor: Getting complicated, hard to follow and maintain. A binding is more better
            if (Math.Abs(CurrentRangeValue - Min) < 0)
            {
                if (TextValue.Visibility == Visibility.Visible)
                {
                    TextMin.Visibility = Visibility.Hidden;
                }
            }
            else if (CurrentRangeValue > Min)
            {
                TextMin.Visibility = Visibility.Visible;
            }
        }

        private void TextValue2_Storyboard_Completed(object sender, EventArgs e)
        {}

        private void PathRangeValue_Loaded(object sender, RoutedEventArgs e)
        {
            var val = CurrentRangeValue / Max;
            var angle = Angle1 + AngleOffset + (TotalAngle * val);
            ValueAngle = angle;
        }

        private void PointAnimationUsingPath_CurrentStateInvalidated(object sender, EventArgs e)
        {
            var clock = (AnimationClock)sender;
            if (clock != null)
            {
                Debug.WriteLine("*** animation time = {0}", clock.CurrentTime);
                Debug.WriteLine("*** animation state = {0}", clock.CurrentState);
                
                if (clock.CurrentState == ClockState.Filling)
                {
                    if (_IsAnimationToOtherNeeded == true)
                    {
                        // bad!
                        var gauge = Helpers.VisualTree.FindVisualParent<GaugeControl>(this);
                        if (gauge != null)
                        {
                            gauge.UpdateValue(CurrentRangeValue, this);
                        }
                    }
                    else
                    {
                        _IsAnimationToOtherNeeded = true;
                    }
                }
            }
        }

        private void BoxSetPoint_Loaded(object sender, RoutedEventArgs e)
        {
            NeedleTextValue = CurrentRangeValue;
        }

       
    }

    /// <summary>
    /// A VM class for textbox hosting the value of the current value of a range. 
    /// The purpose of this class is to handle the chaning of value thru ENTER key.
    /// </summary>
    public class ValueViewModel : INotifyPropertyChanged
    {
        #region componentmodel_boilerplate
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        public ValueViewModel(Sector sector, double min, double max, double initValue,
            /*bad*/Point? forTxtBoxInNeedle = null)
        {
            Sector = sector;
            Min = min;
            Max = max;
            if (forTxtBoxInNeedle.HasValue == true)
            {
                TextBoxHeight = forTxtBoxInNeedle.Value.Y;
                TextBoxWidth = forTxtBoxInNeedle.Value.X;
            }
            UpdateValue(initValue);
        }

        public Sector Sector { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }

        public double TextBoxWidth { get; set; }
        public double TextBoxHeight { get; set; }

        // This is bad design *********************
        double _Top;
        public double Top
        {
            get { return _Top; }
            set
            {
                _Top = value;
                OnPropertyChanged("Top");
            }
        }

        double _Left;
        public double Left
        {
            get { return _Left; }
            set
            {
                _Left = value;
                OnPropertyChanged("Left");
            }
        }
        // This is bad design *********************

        Point _TextPoint;
        public Point TextPoint
        {
            get { return _TextPoint; }
            set
            {
                _TextPoint = value;
                OnPropertyChanged("TextPoint");
            }
        }

        Point _NeedlePointStart;
        public Point NeedlePointStart
        {
            get { return _NeedlePointStart; }
            set
            {
                _NeedlePointStart = value;
                OnPropertyChanged("NeedlePointStart");
            }
        }

        Point _ValuePoint;
        public Point ValuePoint
        {
            get { return _ValuePoint; }
            set
            {
                _ValuePoint = value;
                OnPropertyChanged("ValuePoint");
            }
        }

        double _Value;
        public double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnPropertyChanged("Value");
            }
        }

        public bool UpdateValue(double value)
        {
            if (value >= Min && value <= Max)
            {
                Value = value;
                var percentage = value / Max;
                var angle = Sector.AngleStart + (Sector.TotalAngle * percentage);
                ValuePoint = Helpers.GeometryHelper.CalculatePoint(Sector.Center.X, Sector.Center.Y, angle, Sector.Radius);
                NeedlePointStart = Helpers.GeometryHelper.CalculatePoint(Sector.Center.X, Sector.Center.Y, angle, 23); // 23 is the core radius
                var textPoint = Helpers.GeometryHelper.CalculatePoint(Sector.Center.X, Sector.Center.Y, angle, Sector.Radius / 2);
                Top = textPoint.Y - TextBoxHeight / 2;
                Left = textPoint.X - TextBoxWidth / 2;
                return true;
            }
            else
            {
                // out of range
                //throw new ArgumentOutOfRangeException("Value is out of range!");
                MessageBox.Show("Out of Range");
                return false;
            }
        }
    }

}
