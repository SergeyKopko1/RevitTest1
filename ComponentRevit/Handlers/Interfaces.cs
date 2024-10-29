using RevitTest.ViewModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RevitTest.ComponentRevit.Handlers
{
    public interface IChangeElementService
    {
        ICollection<IFamilyTypeViewModel> SelectedItems { get; set; }
        Task ExecuteChangeAsync();
    }

    public interface IPickElementService
    {
        Task ExecutePickAsync();
    }

}

