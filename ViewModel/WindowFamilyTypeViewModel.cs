using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RevitTest.ViewModel;

internal class WindowFamilyTypeViewModel : ObservableObject, IFamilyTypeViewModel
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