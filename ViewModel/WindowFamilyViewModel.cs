using Autodesk.Revit.DB;

namespace RevitTest.ViewModel
{
    internal class WindowFamilyViewModel : IFamilyViewModel
    {
        public string Name { get; }
        public ElementId Id { get; }

        public WindowFamilyViewModel(string name, ElementId id)
        {
            Name = name;
            Id = id;
        }
    }
}