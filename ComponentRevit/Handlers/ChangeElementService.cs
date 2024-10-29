using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RevitTest.ComponentRevit.Handlers;

public class ChangeElementService : IChangeElementService
{
    private readonly AsyncEventHandler _eventHandler;
    public ICollection<IFamilyTypeViewModel> SelectedItems { get; set; } = new Collection<IFamilyTypeViewModel>();

    public ChangeElementService(AsyncEventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public async Task ExecuteChangeAsync()
    {
        await _eventHandler.RaiseAsync(async app =>
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
                        var collector = doc.GetElement(item.Id);

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
        });
    }
}
