namespace RevitTest.Interfaces
{
    public interface IPickElement
    {
        event Action ElementsCleared;
        event Action<IFamilyTypeViewModel> ElementsAdded;
        Task ExecutePickAsync();
    }
}
