using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.Utils;
using RevitTest.Services;
using RevitTest.ViewModel;


namespace RevitTest.Interfaces
{
    public class PickElementInterface : IPickElementService
    {
        private readonly AsyncEventHandler _eventHandler;

        public event Action ElementsCleared;
        public event Action<IFamilyTypeViewModel> ElementsAdded;

        public PickElementInterface(AsyncEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task ExecutePickAsync()
        {
            await _eventHandler.RaiseAsync( app =>
            {
                var uidoc = app.ActiveUIDocument;
                var doc = uidoc.Document;

                try
                {
                    var references = uidoc.Selection.PickObjects(ObjectType.Element, new SelectionsFilter(
                        e => e is FamilyInstance fi &&
                             fi.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows
                    ));

                    var selectedElementIds = references.Select(r => r.ElementId).ToList();


                    ElementsCleared?.Invoke();

                    HashSet<ElementId> uniqueTypes = new();
                    foreach (var elementId in selectedElementIds)
                    {
                        var windowFromDoc = doc.GetElement(elementId);
                        var windowTypeFromDoc = windowFromDoc.GetTypeId();
                        uniqueTypes.Add(windowTypeFromDoc);
                    }

                    foreach (var uniqueTypeId in uniqueTypes)
                    {
                        var windowTypeElement = doc.GetElement(uniqueTypeId) as FamilySymbol;
                        if (windowTypeElement != null)
                        {
                            string name = windowTypeElement.Name;
                            var viewModel = new WindowFamilyTypeViewModel(name, uniqueTypeId);


                            ElementsAdded?.Invoke(viewModel);
                        }
                    }
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {

                }
            });
        }

        public string GetName()
        {
            return "Выбор элементов";
        }
    }
}
