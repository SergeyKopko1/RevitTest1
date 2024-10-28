using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitTest.ComponentRevit.Extensions.ExtenstionSelections;
using RevitTest.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace RevitTest.ComponentRevit.Handlers

{
    internal class PickElementHandler : IExternalEventHandler
    {
        private readonly MainViewModel _mainViewModel;

        public PickElementHandler(MainViewModel viewModel)
        {
            _mainViewModel = viewModel;
        }

        public void Execute(UIApplication app)
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


                _mainViewModel.ClearRevitElements();

                HashSet<ElementId> uniqueTypes = new HashSet<ElementId>();

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
                        _mainViewModel.AddRevitElement(viewModel);
                    }
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {

            }
        }

        public string GetName()
        {
            return "Выбор элементов";
        }
    }

}
