using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitTest.Interfaces;
using RevitTest.Services;
using RevitTest.View;

namespace RevitTest.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPickElementService _pickElementService;
        private readonly IChangeElementService _changeElementService;


        private readonly SettingsViewModel _settingsViewModel;

        public Dispatcher UiDispatcher { get; set; }

        [ObservableProperty] private ObservableCollection<IFamilyTypeViewModel> _revitElements = [];

        [ObservableProperty] private ObservableCollection<IFamilyTypeViewModel> _selectedItems = [];

        public MainViewModel(IPickElementService pickElementService, IChangeElementService changeElementService)
        {
            _pickElementService = pickElementService;
            _changeElementService = changeElementService;

            _settingsViewModel = new SettingsViewModel();

            _pickElementService.ElementsCleared += ClearRevitElements;
            _pickElementService.ElementsAdded += AddRevitElement;
        
            if (_changeElementService is ChangeElementInterface changeElementInterface)
            {
                changeElementInterface.SetSettingsViewModel(_settingsViewModel);
            }
        }

        [RelayCommand]
        private void OpenSettings()
        {
            var settingsWindow = new Settings
            {
                DataContext = _settingsViewModel 
            };
            settingsWindow.ShowDialog();
        }

        [RelayCommand]
        private async void OnPick()
        {
            await _pickElementService.ExecutePickAsync();
        }


        [RelayCommand]
        private async void OnChange(IList selectedItems)
        {
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов для изменения.");
                return;
            }

            _selectedItems.Clear();
            foreach (IFamilyTypeViewModel item in selectedItems)
            {
                _selectedItems.Add(item);
            }
            _changeElementService.SelectedItems = _selectedItems;

            await _changeElementService.ExecuteChangeAsync();
        }

        public void AddRevitElement(IFamilyTypeViewModel element)
        {
            UiDispatcher.Invoke(() =>
            {
                _revitElements.Add(element);
                OnPropertyChanged(nameof(_revitElements));
            });
        }

        public void ClearRevitElements()
        {
            UiDispatcher.Invoke(() =>
            {
                _revitElements.Clear();
                OnPropertyChanged(nameof(_revitElements));
            });
        }
    }

}
