using RevitTest.Model;


namespace RevitTest.Interfaces
{
    public interface IChangeElement
    {
        ICollection<IFamilyTypeViewModel> SelectedItems { get; set; }
        Task ExecuteChangeAsync(AppSettings settings);
    }
}
