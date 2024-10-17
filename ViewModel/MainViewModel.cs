using RevitTest.ViewModel;
using System.Collections.ObjectModel;

public partial class MainViewModel : ViewModelBase
{


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


}