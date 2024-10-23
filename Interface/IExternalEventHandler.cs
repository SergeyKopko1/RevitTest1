using System.Collections.ObjectModel;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitTest.ViewModel;

namespace RevitTest.Interface
{
    public class IChangeElementHandler : ViewModelBase, IExternalEventHandler
    {
        public ObservableCollection<IFamilyViewModel> RevitElements { get; } = new ObservableCollection<IFamilyViewModel>();

        public void Execute(UIApplication app)
        {
            var uidoc = app.ActiveUIDocument;
            var doc = uidoc.Document;

            using (Transaction trans = new Transaction(doc, "Change Element"))
            {
                trans.Start();

                if (RevitElements.Count == 0)
                {
                    MessageBox.Show("Нет выделенных элементов для изменения размера.");
                    return;
                }

                MessageBox.Show($"Изменение размеров для {RevitElements.Count} окон."); 

                foreach (var item in RevitElements)
                {
                    if (item is WindowFamilyViewModel)
                    {
                        var window = uidoc.Document.GetElement(item.Id) as FamilyInstance;

                        if (window == null)
                        {
                            MessageBox.Show($"Окно с ID {item.Id} не найдено.");
                            continue;
                        }

                        var widthWindow = window.get_Parameter(BuiltInParameter.WINDOW_WIDTH);
                        var heightWindow = window.get_Parameter(BuiltInParameter.WINDOW_HEIGHT);

                        if (widthWindow == null || heightWindow == null)
                        {
                            MessageBox.Show($"Окно с ID {item.Id} не имеет параметров ширины или высоты.");
                            continue;
                        }

                        var newHeight = heightWindow.AsDouble() * 2;
                        var newWidth = widthWindow.AsDouble() * 2;

                        widthWindow.Set(newWidth);
                        heightWindow.Set(newHeight);
                    }
                }

                trans.Commit();
            }
        }


        public string GetName()
        {
            return "Change Element External Event Handler";
        }
    }
}