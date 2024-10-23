using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitTest.ViewModel;
using System.Windows;

namespace RevitTest.Interface
{
    internal class IChangeElementHandler : IExternalEventHandler
    {
        private readonly MainViewModel _mainViewModel;

        public IChangeElementHandler(MainViewModel viewModel)
        {
            _mainViewModel = viewModel;
        }

        public void Execute(UIApplication app)
        {
            var uidoc = app.ActiveUIDocument;
            var doc = uidoc.Document;

            try
            {
                var selectedItems = _mainViewModel.SelectedItems;

                if (selectedItems.Count == 0)
                {
                    MessageBox.Show("Нет выделенных элементов для изменения.");
                    return;
                }

                using (Transaction trans = new Transaction(doc, "Изменение окон"))
                {
                    trans.Start();

                    foreach (var item in selectedItems)
                    {
                        if (item is WindowFamilyViewModel window)
                        {
                            var element = doc.GetElement(window.Id) as FamilyInstance;
                            if (element == null)
                            {
                                MessageBox.Show($"Элемент с ID {window.Id} не найден.");
                                continue;
                            }

                            var widthParam = element.get_Parameter(BuiltInParameter.WINDOW_WIDTH);
                            var heightParam = element.get_Parameter(BuiltInParameter.WINDOW_HEIGHT);

                            if (widthParam != null && heightParam != null)
                            {
                                var newWidth = widthParam.AsDouble() * 1.1;  
                                var newHeight = heightParam.AsDouble() * 1.1; 
                                widthParam.Set(newWidth);
                                heightParam.Set(newHeight);
                            }
                        }
                    }

                    trans.Commit();
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
            }
        }

        public string GetName()
        {
            return "Изменение элементов";
        }
    }
}
