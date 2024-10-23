using Autodesk.Revit.DB;

namespace RevitTest.ViewModel
{
    internal class WindowFamilyViewModel : ViewModelBase, IFamilyViewModel
    {
        public WindowFamilyViewModel(string name, ElementId id, bool isSelected)
        {
            Name = name;
            Id = id;
            IsSelected = isSelected;
        }

        public string Name { get; }
        public ElementId Id { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}