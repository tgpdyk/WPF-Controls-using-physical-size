using System.Windows;
using System.Windows.Input;

namespace DisplayDeviceInfo
{
    /// <summary>
    /// Interaction logic for ManualSizeInputDialog.xaml
    /// </summary>
    public partial class ManualSizeInputDialog : Window
    {
        public ManualSizeInputDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this.InputMonitorSize);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InputMonitorSize_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
           {
               this.Close();
               e.Handled = true;
           }
        
        }
    }
}
