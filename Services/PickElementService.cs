using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.Interfaces;
using RevitTest.Utils;
using RevitTest.ViewModel;


namespace RevitTest.Services
{
    public class PickElementService : IPickElementInterface
    {
        private readonly AsyncEventHandler _eventHandler;
        private MainViewModel _mainViewModel;

        public event Action ElementsCleared;
        public event Action<IFamilyTypeViewModel> ElementsAdded;

        public PickElementService(AsyncEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task ExecutePickAsync()
        {
            await _eventHandler.RaiseAsync(app =>
            {
                var uidoc = app.ActiveUIDocument;
                var doc = uidoc.Document;

                try
                {
                    var references = uidoc.Selection.PickObjects(ObjectType.Element, new SelectionsFilter(
                     e => e is FamilyInstance fi &&
                          (fi.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows ||
                           fi.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors)
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
                            string category = windowTypeElement.Category.Name;
                            string name = windowTypeElement.Name;
                            var viewModel = new WindowFamilyTypeViewModel(name, uniqueTypeId, category);


                            ElementsAdded?.Invoke(viewModel);
                        }
                    }
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {

                }
            });
        }
    }
}
