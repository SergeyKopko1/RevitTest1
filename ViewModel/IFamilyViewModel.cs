using Autodesk.Revit.DB;
namespace RevitTest.ViewModel
{
    public interface IFamilyViewModel
    {
        string Name { get; }
        ElementId Id { get; }
        bool IsSelected { get; set; }
     
    }
}