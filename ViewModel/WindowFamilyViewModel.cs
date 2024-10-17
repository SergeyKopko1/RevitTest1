using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RevitTest.ViewModel
{
    internal class WindowFamilyViewModel : IFamilyViewModel
    {
        public string Name { get; }
        public ElementId Id { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public WindowFamilyViewModel(string name, ElementId id, bool isSelected)
        {
            Name = name;
            Id = id;
            IsSelected = isSelected;
        }

    }
}
