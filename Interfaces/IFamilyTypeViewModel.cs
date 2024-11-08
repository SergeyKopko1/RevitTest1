using Autodesk.Revit.DB;
namespace RevitTest.Interfaces;

public interface IFamilyTypeViewModel
{
    string Name { get; }
    ElementId Id { get; }

    string CategoryElement { get; }

    bool IsSelected { get; set; }
}