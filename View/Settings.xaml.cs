using System.Windows;


namespace RevitTest.View
{
 
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            
        }

        private void closeClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
