using Autodesk.Revit.DB;
namespace RevitTest.ViewModel
{
    public interface IFamilyTypeViewModel
    {
        string Name { get; }
        ElementId Id { get; }
        bool IsSelected { get; set; }

        double Width { get; }

        double Height { get; }
     
    }
}