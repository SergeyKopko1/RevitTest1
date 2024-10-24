using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitTest.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RevitTest.Handlers
{
    public class ChangeElementHandler : IExternalEventHandler
    {
        public ICollection<IFamilyTypeViewModel> SelectedItems { get; set; } = new Collection<IFamilyTypeViewModel>();

            public void Execute(UIApplication app)
            {
                var uidoc = app.ActiveUIDocument;
                var doc = uidoc.Document;

                using (Transaction trans = new Transaction(doc, "Change Element"))
                {
                    trans.Start();

                
                    if (SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Нет выбранных элементов для изменения.");
                        return;
                    }

                foreach (var item in SelectedItems)
                {
                    if (item is WindowFamilyTypeViewModel windowFamily)
                    {
          
                        var collector = new FilteredElementCollector(doc)
                            .OfClass(typeof(FamilySymbol))
                            .Cast<FamilySymbol>()
                            .FirstOrDefault(f => f.Name == windowFamily.Name);

                        if (collector == null)
                        {
                            MessageBox.Show($"Тип окна с именем {windowFamily.Name} не найден.");
                            continue;
                        }

                        var windowsOfType = new FilteredElementCollector(doc)
                            .OfClass(typeof(FamilyInstance))
                            .WhereElementIsNotElementType()
                            .Cast<FamilyInstance>()
                            .Where(w => w.Symbol.Id == collector.Id)
                            .ToList();

                        if (!windowsOfType.Any())
                        {
                            MessageBox.Show($"Не найдено экземпляров окон типа {windowFamily.Name}.");
                            continue;
                        }

        
                        var widthParam = collector.get_Parameter(BuiltInParameter.WINDOW_WIDTH);
                        var heightParam = collector.get_Parameter(BuiltInParameter.WINDOW_HEIGHT);

                        if (widthParam == null || heightParam == null)
                        {
                            MessageBox.Show($"Тип окна {windowFamily.Name} не имеет параметров ширины или высоты.");
                            continue;
                        }

                        var newHeight = heightParam.AsDouble() * 2;
                        var newWidth = widthParam.AsDouble() * 2;

                        widthParam.Set(newWidth);
                        heightParam.Set(newHeight);
                    }
                }

                trans.Commit();
                }

            MessageBox.Show($"Изменение размеров для {SelectedItems.Count} окон завершено.");
        }

        public string GetName()
        {
            return "Change Element External Event Handler";
        }
    }
}
