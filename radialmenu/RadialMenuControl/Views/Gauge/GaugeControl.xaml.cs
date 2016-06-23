using System.Windows;
using System.Windows.Controls;

namespace RadialMenuControl.Views.Gauge
{
    /// <summary>
    /// Interaction logic for GaugeControl.xaml
    /// </summary>
    public partial class GaugeControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsAnimateNeedleEditBoxProperty =
        DependencyProperty.Register("IsAnimateNeedleEditBox",
        typeof(bool),
        typeof(GaugeControl),
        new PropertyMetadata(false, OnIsAnimateEditValueChanged));

        public bool IsAnimateNeedleEditBox
        {
            get { return (bool)GetValue(IsAnimateNeedleEditBoxProperty); }
            set { SetValue(IsAnimateNeedleEditBoxProperty, value); }
        }

        private static void OnIsAnimateEditValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gaugeCtrl = d as GaugeControl;
            if (gaugeCtrl != null)
            {
                bool isAnimate = (bool)e.NewValue;
                gaugeCtrl.ProcessValue.AnimateNeedleBox = isAnimate;
                gaugeCtrl.Output.AnimateNeedleBox = isAnimate;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsSPTrackingEnabledProperty =
        DependencyProperty.Register("IsSPTrackingEnabled",
        typeof(bool),
        typeof(GaugeControl),
        new PropertyMetadata(true, OnIsSPTrackingEnabledValueChanged));

        public bool IsSPTrackingEnabled
        {
            get { return (bool)GetValue(IsSPTrackingEnabledProperty); }
            set { SetValue(IsSPTrackingEnabledProperty, value); }
        }

        private static void OnIsSPTrackingEnabledValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gaugeCtrl = d as GaugeControl;
            if (gaugeCtrl != null)
            {
                if (gaugeCtrl.SelectedMode == GaugeModeEnum.Manual)
                {
                    bool isTracking = (bool)e.NewValue;
                    gaugeCtrl.ProcessValue.IsEditable = (isTracking == false)? true : false;
                    gaugeCtrl.ProcessValue.TextValue.Visibility = Visibility.Visible;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedModeProperty =
               DependencyProperty.Register("SelectedMode",
               typeof(GaugeModeEnum),
               typeof(GaugeControl),
               new PropertyMetadata(GaugeModeEnum.Auto, OnChangeMode));

        public GaugeModeEnum SelectedMode
        {
            get { return (GaugeModeEnum)GetValue(SelectedModeProperty); }
            set { SetValue(SelectedModeProperty, value); }
        }

        private static void OnChangeMode(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GaugeModeEnum mode = (GaugeModeEnum)e.NewValue;
            GaugeControl ctrl = d as GaugeControl;

            switch (mode)
            {
                case GaugeModeEnum.Cascade:
                    {
                        // Needle
                        ctrl.Output.NeedleVisibility = Visibility.Visible;
                        ctrl.ProcessValue.NeedleVisibility = Visibility.Visible;
                        // TextValue
                        ctrl.Output.TextValue.Visibility = Visibility.Visible;
                        ctrl.ProcessValue.TextValue.Visibility = Visibility.Visible;
                        // TextBox editable or not
                        ctrl.Output.IsEditable = false;
                        ctrl.ProcessValue.IsEditable = false;
                    }
                    break;
                case GaugeModeEnum.Manual:
                    {
                        // Needle
                        ctrl.Output.NeedleVisibility = Visibility.Visible;
                        ctrl.ProcessValue.NeedleVisibility = Visibility.Visible;
                        // TextValue
                        ctrl.Output.TextValue.Visibility = Visibility.Hidden;
                        ctrl.ProcessValue.TextValue.Visibility = Visibility.Hidden;

                        // TextBox editable or not
                        ctrl.Output.IsEditable = true;
                        
                        ctrl.ProcessValue.IsEditable = (ctrl.IsSPTrackingEnabled == false) ? true : false;;

                        if (ctrl.IsSPTrackingEnabled == true)
                        {
                            ctrl.ProcessValue.TextValue.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ctrl.ProcessValue.TextValue.Visibility = Visibility.Hidden;
                        }
                        
                        

                        ctrl.ProcessValue._IsAnimationToOtherNeeded = true;
                        ctrl.Output._IsAnimationToOtherNeeded = true;


                    }
                    break;
                default: // Auto
                    {
                        // Needle
                        ctrl.Output.NeedleVisibility = Visibility.Hidden;
                        ctrl.ProcessValue.NeedleVisibility = Visibility.Visible;
                        // Text Value
                        ctrl.ProcessValue.TextValue.Visibility = Visibility.Hidden;
                        ctrl.Output.TextValue.Visibility = Visibility.Visible;
                        // TextBox editable or not
                        ctrl.Output.IsEditable = false;
                        ctrl.ProcessValue.IsEditable = true;

                        var currVal = ctrl.ProcessValue.BoxSetPoint.Text;
                        double PV = 0d;
                        if (double.TryParse(currVal.ToString(), out PV))
                        {
                            ctrl.UpdateValue(PV, ctrl.ProcessValue);
                            ctrl.ProcessValue.CurrentRangeValue = PV;
                            
                            ctrl.ProcessValue.UpdateNeedleAndValuePointObject(PV, false);
                        }
                    }
                    break;
            }
        }
        public GaugeControl()
        {
            InitializeComponent();
            // Refactor: Setting of default value

            // AUTO by default
            Output.NeedleVisibility = Visibility.Hidden;
            ProcessValue.NeedleVisibility = Visibility.Visible;
            ProcessValue.TextValue.Visibility = Visibility.Hidden;
        }

        // very bad! output animation counter
        int outputCounter = 0;

        // This is definitely ugly. Coupled. There might be a more elegant way 
        //.. Maybe BINDING, but surely, this should not be the implementation
        // Refactor. What is better than name?
        public void UpdateValue(double value, RangeControl itemSender)
        {

            switch (SelectedMode)
            {
                case GaugeModeEnum.Cascade:
                    {
                       //do nothing
                    }
                    break;
                case GaugeModeEnum.Manual:
                    {
                        if (itemSender == this.Output)
                        {
                            // Do something with ProcessValue
                            if (IsSPTrackingEnabled == true)
                            {
                                if (outputCounter == 0)
                                {
                                    this.Output._IsAnimationToOtherNeeded = true;
                                    this.Output.UpdateValue(value, false, true, 1);
                                    outputCounter++;
                                }
                                else
                                {
                                    double p = (value / this.Output.Max);
                                    var pVal = (100 - (p * 100)) / 100;
                                    double newValue = this.ProcessValue.Max * pVal;
                                    this.ProcessValue._IsAnimationToOtherNeeded = false;
                                    this.ProcessValue.UpdateValue(newValue, false, false, 2, 0);
                                    outputCounter = 0;
                                }
                            }
                            else
                            {
                                if (outputCounter == 0)
                                {
                                    this.Output._IsAnimationToOtherNeeded = true;
                                    this.Output.UpdateValue(value, false, true, 1);
                                    outputCounter++;
                                }
                                else
                                {
                                    double p = (value / this.Output.Max);
                                    var pVal = (100 - (p * 100)) / 100;
                                    double newValue = this.ProcessValue.Max * pVal;
                                    this.ProcessValue._IsAnimationToOtherNeeded = false;
                                    this.ProcessValue.UpdateValue(newValue, false, false, 2, 0);
                                    outputCounter = 0;
                                }
                            }
                        }
                    }
                    break;
                default: // Auto
                    {
                        if (itemSender == this.ProcessValue)
                        {
                            // Do something with Ouput -- and this is in Percentage
                            double p = value / this.ProcessValue.Max * this.Output.Max;
                            var newValue = (p - 100) * -1;
                            this.Output._IsAnimationToOtherNeeded = true;
                            this.Output.UpdateValue(newValue, false, true, 1.5);

                            //this.ProcessValue.UpdateValue(value, false, true, 2, 2);
                        }
                        else if (itemSender == this.Output)
                        {
                            double p = (value / this.Output.Max);
                            var pVal = (100 - (p * 100)) / 100;
                            double newValue = this.ProcessValue.Max * pVal;
                            this.ProcessValue._IsAnimationToOtherNeeded = false;
                            this.ProcessValue.UpdateValue(newValue, false, false, 1.5, 0);
                        }
                    }
                    break;
            }
        }

        public void UpdateNeedle(double value, RangeControl itemSender)
        {
            // This is definitely ugly. Coupled. There might be a more elegant way 
            //.. Maybe BINDING, but surely, this should not be the implementation
            // Refactor. What is better than name.
            if (itemSender == this.Output && this.IsSPTrackingEnabled == true && this.SelectedMode == GaugeModeEnum.Manual)
            {
                double p = (value / this.Output.Max);
                var pVal = (100 - (p * 100)) / 100;
                double newValue = this.ProcessValue.Max * pVal;
                this.ProcessValue.UpdateNeedleAndValuePointObject(newValue, false);
            }
        }


        private int _AnimatedRangeCtr = 0;
        public void UpdateAnimatedRange()
        {
            if (_AnimatedRangeCtr == 2)
            {
                _AnimatedRangeCtr = 0;
            }
        }
    }
}
