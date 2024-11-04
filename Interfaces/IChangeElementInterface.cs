using RevitTest.ViewModel;


namespace RevitTest.Interfaces
{
    public interface IChangeElementInterface
    {
        ICollection<IFamilyTypeViewModel> SelectedItems { get; set; }
        Task ExecuteChangeAsync();
    }
}
