using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RevitTest.Interfaces;
using RevitTest.View;
using RevitTest.Model;

namespace RevitTest.ViewModel
{
    public partial class MainViewModel : ObservableObject, IDisposable
    {
        private readonly IPickElement _pickElementService;
        private readonly IChangeElement _changeElementService;
        private readonly IServiceProvider _serviceProvider;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly AppSettings _appSettings;

        public Dispatcher UiDispatcher { get; set; }

        [ObservableProperty] private ObservableCollection<CategoryGroup> _revitElementsGroupedByCategory = new();
        [ObservableProperty] private ObservableCollection<IFamilyTypeViewModel> _revitElements = new();
        [ObservableProperty] private ObservableCollection<IFamilyTypeViewModel> _selectedItems = new();

        public MainViewModel(
            IPickElement pickElementService, 
            IChangeElement changeElementService, 
            SettingsViewModel settingsViewModel, 
            IServiceProvider serviceProvider,
            AppSettings appSettings
     
            )
        {
            _pickElementService = pickElementService;
            _changeElementService = changeElementService;
            _settingsViewModel = settingsViewModel;
            _serviceProvider = serviceProvider;
            _appSettings = appSettings;

            _pickElementService.ElementsCleared += OnElementsCleared;
            _pickElementService.ElementsAdded += OnElementsAdded;
        }

        public void Dispose()
        {
            if (_pickElementService != null)
            {
                _pickElementService.ElementsCleared -= OnElementsCleared;
                _pickElementService.ElementsAdded -= OnElementsAdded;
            }
        }

        private void OnElementsAdded(IFamilyTypeViewModel el)
        {
            AddRevitElementAndUpdateGroups(el);
        }

        private void OnElementsCleared()
        {
            ClearRevitElements();
        }

        private void UpdateSelectedItems(IFamilyTypeViewModel element)
        {
            if (element.IsSelected)
            {
                if (!SelectedItems.Contains(element))
                {
                    SelectedItems.Add(element);
                }
            }
            else
            {
                SelectedItems.Remove(element);
            }

            OnPropertyChanged(nameof(SelectedItemsCount));
        }
        public int SelectedItemsCount => SelectedItems.Count;

        [RelayCommand]
        private void OpenSettings()
        {
            var settingsView = _serviceProvider.GetRequiredService<SettingsView>();
            var settingsViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();

            settingsViewModel.FamilyTypes = _selectedItems;
            settingsViewModel.IsApplyToWorkset = _appSettings.IsApplyToWorkset;
            settingsView.DataContext = settingsViewModel;

            settingsView.ShowDialog();
        }

        [RelayCommand]
        private async Task OnPick()
        {
            await _pickElementService.ExecutePickAsync();
        }

        [RelayCommand]
        private async Task OnChange()
        {
            if (SelectedItems.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов для изменения.");
                return;
            }

            _changeElementService.SelectedItems = SelectedItems;

            await _changeElementService.ExecuteChangeAsync(_appSettings);
        }

        private void ClearRevitElements()
        {
            UiDispatcher.Invoke(() =>
            {
                RevitElements.Clear();
                OnPropertyChanged(nameof(RevitElements));
            });
        }
         

        private void AddRevitElementAndUpdateGroups(IFamilyTypeViewModel element)
        {
            if (element is FamilyTypeViewModel windowElement)
            {
                windowElement.IsSelectedChanged += UpdateSelectedItems;
            }

            RevitElements.Add(element);
            OnPropertyChanged(nameof(RevitElements));
            UpdateGroupedElements();
        }

        private void UpdateGroupedElements()
        {
            var grouped = RevitElements
                .GroupBy(e => e.CategoryElement)
                .Select(g => new CategoryGroup
                {
                    Category = g.Key,
                    Elements = new ObservableCollection<IFamilyTypeViewModel>(g)
                });

            UiDispatcher.Invoke(() =>
            {
                RevitElementsGroupedByCategory.Clear();
                foreach (var group in grouped)
                {
                    RevitElementsGroupedByCategory.Add(group);
                }
            });
        }
    }

    public class CategoryGroup
    {
        public string Category { get; set; }
        public ObservableCollection<IFamilyTypeViewModel> Elements { get; set; }
    }
}
