using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.Interfaces;
using RevitTest.Utils;
using RevitTest.ViewModel;
using Autodesk.Revit.UI;

namespace RevitTest.Services
{
    public class PickElementService : IPickElement
    {
        private readonly AsyncEventHandler _eventHandler;

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
                    if (!doc.IsWorkshared)
                    {
                        TaskDialog.Show("Ошибка", "Документ не поддерживает рабочие наборы. Пожалуйста, включите Worksharing вручную, чтобы использовать рабочие наборы.");
                        return;
                    }
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
                        var element = doc.GetElement(elementId);
                        var elementTypeId = element.GetTypeId();
                        uniqueTypes.Add(elementTypeId);
                    }
                    var worksetTable = new FilteredWorksetCollector(doc).OfKind(WorksetKind.UserWorkset);
                    _ = worksetTable.FirstOrDefault(w => w.Name == "Окна") ?? CreateWorkset(doc, "Окна");
                    _ = worksetTable.FirstOrDefault(w => w.Name == "Двери") ?? CreateWorkset(doc, "Двери");

                    foreach (var uniqueTypeId in uniqueTypes)
                    {
                        var typeElement = doc.GetElement(uniqueTypeId) as FamilySymbol;
                        if (typeElement != null)
                        {
                            string category = typeElement.Category.Name;
                            string name = typeElement.Name;
                            var viewModel = new FamilyTypeViewModel(name, uniqueTypeId, category);
                            ElementsAdded?.Invoke(viewModel);
                        }
                    }
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                }
            });
        }
        private Workset CreateWorkset(Document doc, string worksetName)
        {
            using (Transaction trans = new Transaction(doc, $"Создание рабочего набора {worksetName}"))
            {
                trans.Start();
                Workset workset = Workset.Create(doc, worksetName);
                trans.Commit();
                return workset;
            }
        }
    }
}
