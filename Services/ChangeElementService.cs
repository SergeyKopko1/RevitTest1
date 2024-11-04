using Autodesk.Revit.DB;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.Interfaces;
using RevitTest.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace RevitTest.Services;

public class ChangeElementService : IChangeElementInterface
{
    private readonly AsyncEventHandler _eventHandler;
    private SettingsViewModel _settingsViewModel;

    public ICollection<IFamilyTypeViewModel> SelectedItems { get; set; } = new Collection<IFamilyTypeViewModel>();

    public ChangeElementService(AsyncEventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public void SetSettingsViewModel(SettingsViewModel settingsViewModel)
    {
        _settingsViewModel = settingsViewModel;
    }

    [Obsolete]
    public async Task ExecuteChangeAsync()
    {
        await _eventHandler.RaiseAsync(app =>
        {
            var uidoc = app.ActiveUIDocument;
            var doc = uidoc.Document;

            using (Transaction trans = new Transaction(doc, "Change Element"))
            {
                trans.Start();

                foreach (var item in SelectedItems)
                {
                    if (item is WindowFamilyTypeViewModel)
                    {
                        var collector = doc.GetElement(item.Id);
                        var widthParam = collector.get_Parameter(BuiltInParameter.WINDOW_WIDTH);
                        var heightParam = collector.get_Parameter(BuiltInParameter.WINDOW_HEIGHT);

                        if (widthParam != null && heightParam != null && _settingsViewModel != null)
                        {


                            double widthDefault = widthParam.AsDouble();
                            double heightDefault = heightParam.AsDouble();

                            if (_settingsViewModel.IsSelectedWidth)
                            {
                                double widthIncrement = UnitUtils.ConvertToInternalUnits(_settingsViewModel.WidthIncrement, DisplayUnitType.DUT_MILLIMETERS);
                                widthParam.Set(widthIncrement);
                            }
                            else
                            {
                                widthParam.Set(widthDefault);
                            }


                            if (_settingsViewModel.IsSelectedHeight)
                            {
                                double heightIncrement = UnitUtils.ConvertToInternalUnits(_settingsViewModel.HeightIncrement, DisplayUnitType.DUT_MILLIMETERS);
                                heightParam.Set(heightIncrement);
                            }
                            else
                            {
                                heightParam.Set(heightDefault);
                            }

                        }
                    }
                }

                trans.Commit();
            }

            MessageBox.Show($"Изменение размеров для {SelectedItems.Count} окон завершено.");
        });
    }
}


