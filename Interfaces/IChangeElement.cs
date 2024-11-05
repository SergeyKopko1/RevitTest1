using RevitTest.Model;
using RevitTest.ViewModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RevitTest.Interfaces
{
    public interface IChangeElement
    {
        ICollection<IFamilyTypeViewModel> SelectedItems { get; set; }
        Task ExecuteChangeAsync(AppSettings settings);
    }
}
