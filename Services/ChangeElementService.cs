using RevitTest.ViewModel;


namespace RevitTest.Services
{
    public interface IChangeElementService
    {
        ICollection<IFamilyTypeViewModel> SelectedItems { get; set; }
        Task ExecuteChangeAsync();
    }
}
