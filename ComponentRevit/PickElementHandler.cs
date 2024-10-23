using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitTest.ComponentRevit.Extensions.ExtenstionSelections;
using RevitTest.ViewModel;
using System.Linq;
using System.Windows;

namespace RevitTest.ComponentRevit
{
    internal class PickElementHandler : IExternalEventHandler
    {
        private MainViewModel _mainViewModel;

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

                foreach (var elementId in selectedElementIds)
                {
                    var windowFromDoc = doc.GetElement(elementId) as FamilyInstance;
                    if (windowFromDoc != null)
                    {
                        var viewModel = new WindowFamilyViewModel(windowFromDoc.Name, windowFromDoc.Id, false);
                        _mainViewModel.AddRevitElement(viewModel);
                    }
                    else
                    {
                        MessageBox.Show($"Элемент с ID {elementId} больше не доступен.");
                    }
                }


                // _mainViewModel.SelectedItems.Clear();
                // foreach (var element in _mainViewModel.RevitElements)
                // {
                //     _mainViewModel.SelectedItems.Add(element);
                // }
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