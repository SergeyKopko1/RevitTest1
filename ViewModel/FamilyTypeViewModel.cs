using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using RevitTest.Interfaces;

namespace RevitTest.ViewModel
{
    internal partial class FamilyTypeViewModel : ObservableObject, IFamilyTypeViewModel
    {
        public event Action<FamilyTypeViewModel> IsSelectedChanged;


        [ObservableProperty] private string _categoryElement;
        [ObservableProperty] private string _icon;
        [ObservableProperty] private string _name;

        public FamilyTypeViewModel(string name, ElementId id, string category)
        {
            Name = name;
            Id = id;
            CategoryElement = category;
            IsSelected = false;

            if (category == "Двери" || category == "Doors")
            {
                Icon = "/RevitTest;component/View/doorIcon.png";
            }
            else
            {
                Icon = "/RevitTest;component/View/windowIcon.png";
            }
        }

        public ElementId Id { get; }

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

    }
}