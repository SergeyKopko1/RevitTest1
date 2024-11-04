using RevitTest.ViewModel;


namespace RevitTest.Interfaces
{
    public interface IPickElementInterface
    {
        event Action ElementsCleared;
        event Action<IFamilyTypeViewModel> ElementsAdded;
        Task ExecutePickAsync();
    }
}
