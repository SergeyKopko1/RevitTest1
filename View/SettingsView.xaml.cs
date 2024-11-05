using System.Windows;


namespace RevitTest.View
{
 
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
            
        }

        private void closeClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
