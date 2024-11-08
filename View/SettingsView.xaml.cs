using System.Windows;


namespace RevitTest.View
{
 
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
            
        }


        private void EnecaTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
          
            e.Handled = !System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[0-9]+$");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
