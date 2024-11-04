using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitTest.View;
using System.Windows;

namespace RevitTest.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty] private double _widthIncrement;
        [ObservableProperty] private double _heightIncrement;
        [ObservableProperty] private bool _isSelectedWidth = true;
        [ObservableProperty] private bool _isSelectedHeight = true;

        [RelayCommand]
        private void Save()
        {
            try
            {
                var settingsWindow = new SettingsView();
                settingsWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
