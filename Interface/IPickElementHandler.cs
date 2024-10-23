using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitTest.ComponentRevit.Extensions.ExtenstionSelections;
using RevitTest.ViewModel;
using System.Linq;
using System.Windows;

namespace RevitTest.Interface
{
    internal class IPickElementHandler : IExternalEventHandler
    {
        private readonly MainViewModel _mainViewModel;

        public IPickElementHandler(MainViewModel viewModel)
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

                foreach (var elementId in selectedElementIds)
                {
                    if (doc.GetElement(elementId) is FamilyInstance windowFromDoc)
                    {
                        var viewModel = new WindowFamilyViewModel(windowFromDoc.Name, windowFromDoc.Id, false);
                        _mainViewModel.AddRevitElement(viewModel);
                    }
                    else
                    {
                        MessageBox.Show($"Элемент с ID {elementId} больше не доступен.");
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
