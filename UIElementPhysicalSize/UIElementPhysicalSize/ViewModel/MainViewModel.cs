using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UIElementInPhysicalSize.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
#region BoilerPlate
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
#endregion
        public MainViewModel()
        {
            GetScreenMonitorInfo();
            PropertyChanged += MainViewModel_PropertyChanged;
        }

        private void GetScreenMonitorInfo()
        {
            var screen = DisplayDeviceInfo.ScreenInfo.Display.CurrentScreen;
            ScreenMonitorName = DisplayDeviceInfo.ScreenInfo.Display.CurrentScreen.Device.Name;
            ScreenMonitorWidth = DisplayDeviceInfo.ScreenInfo.Display.CurrentScreen.Device.GetWidth((double)screen.WidthPx, (double)screen.HeightPx).ToString();
            ScreenMonitorHeight = DisplayDeviceInfo.ScreenInfo.Display.CurrentScreen.Device.GetHeight((double)screen.WidthPx, (double)screen.HeightPx).ToString();
            ScreenMonitorSize = DisplayDeviceInfo.ScreenInfo.Display.CurrentScreen.Device.GetSize().ToString();
            ScreenMonitorPpi = DisplayDeviceInfo.ScreenInfo.Display.CurrentScreen.Device.GetXPpi((double)screen.WidthPx, (double)screen.HeightPx).ToString();
        }

        private bool _isNewScreenDetected;
        private string _screenMonitorName;
        private string _screenMonitorHeight;
        private string _screenMonitorWidth;
        private string _screenMonitorSize;
        private string _screenMonitorPpi;

        public bool IsNewScreenDetected
        {
            get { return _isNewScreenDetected; }
            set
            {
                if (_isNewScreenDetected != value)
                {
                    _isNewScreenDetected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string ScreenMonitorName
        {
            get { return _screenMonitorName; }
            set
            {
                _screenMonitorName = value;
                NotifyPropertyChanged();
            }
        }

        public string ScreenMonitorHeight
        {
            get { return _screenMonitorHeight; }
            set
            {
                _screenMonitorHeight = value;
                NotifyPropertyChanged();
            }
        }

        public string ScreenMonitorWidth
        {
            get { return _screenMonitorWidth; }
            set
            {
                _screenMonitorWidth = value;
                NotifyPropertyChanged();
            }
        }

        public string ScreenMonitorSize
        {
            get { return _screenMonitorSize; }
            set
            {
                _screenMonitorSize = value;
                NotifyPropertyChanged();
            }
        }

        public string ScreenMonitorPpi
        {
            get { return _screenMonitorPpi; }
            set
            {
                _screenMonitorPpi = value;
                NotifyPropertyChanged();
            }
        }

        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsNewScreenDetected"))
            {
                GetScreenMonitorInfo();
            }
        }
    }
}
