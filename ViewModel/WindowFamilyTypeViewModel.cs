using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace RevitTest.ViewModel
{
    internal class WindowFamilyTypeViewModel : ObservableObject, IFamilyTypeViewModel
    {
        public event Action<WindowFamilyTypeViewModel> IsSelectedChanged;

        public WindowFamilyTypeViewModel(string name, ElementId id, string category)
        {
            Name = name;
            Id = id;
            Category = category;
            IsSelected = false;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged(nameof(_isSelected));
                IsSelectedChanged?.Invoke(this);
            }
        }

        private string _name;
        private string _category;

        public string Category
        {
            get => _category;
            set
            {
                if (_category == value) return;
                _category = value;
                OnPropertyChanged(nameof(_category));
            }
        }

        public ElementId Id { get; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged(nameof(_name));
            }
        }
    }
}