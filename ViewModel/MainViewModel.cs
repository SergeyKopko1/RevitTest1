using Autodesk.Revit.UI;
using RevitTest.ComponentRevit;
using RevitTest.ComponentRevit.Handlers;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RevitTest.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private PickElementHandler _pickElementHandler;
        private ExternalEvent _pickElementEvent;

        private ChangeElementHandler _changeElementHandler;
        private ExternalEvent _changeElementEvent;

        private ObservableCollection<IFamilyTypeViewModel> _revitElements = new ObservableCollection<IFamilyTypeViewModel>();
        public ObservableCollection<IFamilyTypeViewModel> RevitElements
        {
            get => _revitElements;
            set
            {
                _revitElements = value;
                OnPropertyChanged(nameof(RevitElements));
            }
        }

        private ObservableCollection<IFamilyTypeViewModel> _selectedItems = new ObservableCollection<IFamilyTypeViewModel>();
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

        public MainViewModel()
        {
            _pickElementHandler = new PickElementHandler(this);
            _pickElementEvent = ExternalEvent.Create(_pickElementHandler);

            _changeElementHandler = new ChangeElementHandler();
            _changeElementEvent = ExternalEvent.Create(_changeElementHandler);

            PickCommand = new RelayCommand(OnPickCommandExecuted);
            ChangeCommand = new RelayCommand<IList>(OnChangeCommandExecuted);
        }
        
        private void OnPickCommandExecuted()
        {
            _pickElementEvent.Raise();
        }


        private void OnChangeCommandExecuted(IList selectedItems)
        {
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов для изменения.");
                return;
            }

            SelectedItems.Clear();
            foreach (IFamilyTypeViewModel item in selectedItems)
            _changeElementEvent.Raise();
        }


        public void AddRevitElement(IFamilyTypeViewModel element)
        {
            RevitElements.Add(element);
            OnPropertyChanged(nameof(RevitElements));
        }

        public void ClearRevitElements()
        {
            RevitElements.Clear();
            OnPropertyChanged(nameof(RevitElements));
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