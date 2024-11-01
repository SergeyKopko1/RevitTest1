using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitTest.View;
using System.Windows;

namespace RevitTest.ViewModel
{
    public partial class SettingsViewModel : ObservableObject

    
    {
        [ObservableProperty] public  double _widthIncrement = new();

        [ObservableProperty] public double _heightIncrement = new();

        [ObservableProperty] public  bool _isSelectedWidth = true;

        [ObservableProperty] public  bool _isSelectedHeight = true;



        [RelayCommand]
        private void Save()
        {
            try
            {
                var settingsWindow = new Settings();
                settingsWindow.Close();
         
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}