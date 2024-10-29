using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.ComponentRevit.Handlers;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace RevitTest.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IPickElementService _pickElementService;
        private readonly IChangeElementService _changeElementService;
        public Dispatcher UiDispatcher { get; set; }

        private ObservableCollection<IFamilyTypeViewModel> _revitElements = new();
        public ObservableCollection<IFamilyTypeViewModel> RevitElements
        {
            get => _revitElements;
            set
            {
                _revitElements = value;
                OnPropertyChanged(nameof(RevitElements));
            }
        }

        private ObservableCollection<IFamilyTypeViewModel> _selectedItems = new();
        public ObservableCollection<IFamilyTypeViewModel> SelectedItems
        {
            get => _selectedItems;
            set
            {
                _selectedItems = value;
                OnPropertyChanged(nameof(SelectedItems));
            }
        }

        public ICommand PickCommand { get; }
        public ICommand ChangeCommand { get; }

        public MainViewModel(IPickElementService pickElementService, IChangeElementService changeElementService)
        {
            _pickElementService = pickElementService;
            _changeElementService = changeElementService;

            PickCommand = new RelayCommand(OnPickCommandExecuted);
            ChangeCommand = new RelayCommand<IList>(OnChangeCommandExecuted);
        }

        private async void OnPickCommandExecuted()
        {
            await _pickElementService.ExecutePickAsync();
        }

        private async void OnChangeCommandExecuted(IList selectedItems)
        {
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов для изменения.");
                return;
            }

            SelectedItems.Clear();
            foreach (IFamilyTypeViewModel item in selectedItems)
            {
                SelectedItems.Add(item);
            }
            _changeElementService.SelectedItems = SelectedItems;

            await _changeElementService.ExecuteChangeAsync();
        }

        public void AddRevitElement(IFamilyTypeViewModel element)
        {
            UiDispatcher.Invoke(() =>
            {
                RevitElements.Add(element);
                OnPropertyChanged(nameof(RevitElements));
            });
        }

        public void ClearRevitElements()
        {
            UiDispatcher.Invoke(() =>
            {
                RevitElements.Clear();
                OnPropertyChanged(nameof(RevitElements));
            });
        }

        public class RelayCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool> _canExecute;
            public event EventHandler CanExecuteChanged;

            public RelayCommand(Action execute, Func<bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

            public void Execute(object parameter) => _execute?.Invoke();
        }

        public class RelayCommand<T> : ICommand
        {
            private readonly Action<T> _execute;
            private readonly Func<T, bool> _canExecute;

            public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute((T)parameter);
            }

            public void Execute(object parameter)
            {
                _execute((T)parameter);
            }

            public event EventHandler CanExecuteChanged;

            public void RaiseCanExecuteChanged()
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
