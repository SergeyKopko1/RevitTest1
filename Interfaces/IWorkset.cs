using System.Collections.ObjectModel;

namespace RevitTest.Interfaces
{
    public interface IWorkset
    {
        Task AssignElementsToWorksetAsync(ObservableCollection<IFamilyTypeViewModel> elementIds);
    }
}
