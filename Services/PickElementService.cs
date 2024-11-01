using RevitTest.ViewModel;


namespace RevitTest.Services
{
    public interface IPickElementService
    {
        event Action ElementsCleared;
        event Action<IFamilyTypeViewModel> ElementsAdded;
        Task ExecutePickAsync();
    }
}
