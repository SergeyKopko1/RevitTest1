using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitTest.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RevitTest.View.Controls
{
    public partial class ListBoxControl : UserControl
    {
        public ListBoxControl() 
        {
            InitializeComponent();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is StackPanel stackPanel && stackPanel.DataContext is WindowFamilyViewModel viewModel)
            {
                viewModel.IsSelected = !viewModel.IsSelected;

            }
        }
    }
}
