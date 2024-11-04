using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitTest.Interfaces;
using RevitTest.Services;
using RevitTest.View;

namespace RevitTest.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPickElementInterface _pickElementService;
        private readonly IChangeElementInterface _changeElementService;


        private readonly SettingsViewModel _settingsViewModel;

        public Dispatcher UiDispatcher { get; set; }

        [ObservableProperty] private ObservableCollection<CategoryGroup> _revitElementsGroupedByCategory = new();

        [ObservableProperty] private ObservableCollection<IFamilyTypeViewModel> _revitElements = [];

        [ObservableProperty] private ObservableCollection<IFamilyTypeViewModel> _selectedItems = [];

        public MainViewModel(IPickElementInterface pickElementService, IChangeElementInterface changeElementService, SettingsViewModel settingsViewModel)
        {
            _pickElementService = pickElementService;
            _changeElementService = changeElementService;
            _settingsViewModel = settingsViewModel;



            _pickElementService.ElementsCleared += ClearRevitElements;
            _pickElementService.ElementsAdded += AddRevitElementAndUpdateGroups;

            if (_changeElementService is ChangeElementService changeElementInterface)
            {
                changeElementInterface.SetSettingsViewModel(_settingsViewModel);
            }
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
            var settingsWindow = new SettingsView
            {
                DataContext = _settingsViewModel
            };
            settingsWindow.ShowDialog();
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

            await _changeElementService.ExecuteChangeAsync();
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
            if (element is WindowFamilyTypeViewModel windowElement)
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
                .GroupBy(e => e.Category)
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