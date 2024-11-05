using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitTest.Model;
using System.Windows;

namespace RevitTest.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty] private double _widthIncrement = new();
        [ObservableProperty] private double _heightIncrement = new();
        [ObservableProperty] private bool _isSelectedWidth = true;
        [ObservableProperty] private bool _isSelectedHeight = true;
        private AppSettings _appSettings;

        public AppSettings AppSettings { get; private set; }

        public SettingsViewModel(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        [RelayCommand]
        private void Save()
        {
            try
            {
                _appSettings.WidthIncrement = WidthIncrement;
                _appSettings.HeightIncrement = HeightIncrement;
                _appSettings.IsSelectedWidth = IsSelectedWidth;
                _appSettings.IsSelectedHeight = IsSelectedHeight;

                MessageBox.Show("Настройки сохранены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
