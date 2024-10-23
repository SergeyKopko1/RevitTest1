﻿using Autodesk.Revit.UI;
using RevitTest.Interface;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RevitTest.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IPickElementHandler _pickElementHandler;
        private ExternalEvent _pickElementEvent;

        private IChangeElementHandler _changeElementHandler;
        private ExternalEvent _changeElementEvent;

        private ObservableCollection<IFamilyViewModel> _revitElements = new ObservableCollection<IFamilyViewModel>();
        public ObservableCollection<IFamilyViewModel> RevitElements
        {
            get => _revitElements;
            set
            {
                _revitElements = value;
                OnPropertyChanged(nameof(RevitElements));
            }
        }

        private ObservableCollection<IFamilyViewModel> _selectedItems = new ObservableCollection<IFamilyViewModel>();
        public ObservableCollection<IFamilyViewModel> SelectedItems
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
            _pickElementHandler = new IPickElementHandler(this);
            _pickElementEvent = ExternalEvent.Create(_pickElementHandler);

            _changeElementHandler = new IChangeElementHandler();
            _changeElementEvent = ExternalEvent.Create(_changeElementHandler);

            PickCommand = new RelayCommand(OnPickCommandExecuted);
            ChangeCommand = new RelayCommand(OnChangeCommandExecuted);
        }

        private void OnPickCommandExecuted()
        {
            _pickElementEvent.Raise();
        }

        private void OnChangeCommandExecuted()
        {
            if (SelectedItems == null)
            {
                MessageBox.Show("SelectedItems не инициализирован.");
                return;
            }

            if (SelectedItems.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов для изменения11.");
                return;
            }

           

            _changeElementEvent.Raise();
        }


        public void AddRevitElement(IFamilyViewModel element)
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

    }
}
