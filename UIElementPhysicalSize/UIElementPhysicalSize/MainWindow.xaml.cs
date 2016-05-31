using System;
using System.Windows;
using DisplayDeviceInfo.AttachedProps;
using UIElementInPhysicalSize.ViewModel;

namespace UIElementInPhysicalSize
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new MainViewModel();
        }

        private void MainWindow_OnLayoutUpdated(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }
    }
}
