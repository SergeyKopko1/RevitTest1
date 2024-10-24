using Autodesk.Revit.DB;

namespace RevitTest.ViewModel
{
    internal class WindowFamilyTypeViewModel : ViewModelBase, IFamilyTypeViewModel
    {
        public WindowFamilyTypeViewModel(string name, ElementId id, bool isSelected, double width, double height)
        {
            Name = name;
            Width = width;
            Height = height;
            Id = id;
            IsSelected = isSelected;
        }

        public string Name { get; }

        public double Width { get; }

        public double Height { get; }
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