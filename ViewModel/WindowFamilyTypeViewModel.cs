using Autodesk.Revit.DB;

namespace RevitTest.ViewModel;

internal class WindowFamilyTypeViewModel : ViewModelBase, IFamilyTypeViewModel
{
    public WindowFamilyTypeViewModel(string name, ElementId id)
    {
        Name = name;

        Id = id;
  
    }

    private string _name;

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