using Autodesk.Revit.DB;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.Services;
using RevitTest.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace RevitTest.Interfaces;

public class ChangeElementInterface : IChangeElementService
{
    private readonly AsyncEventHandler _eventHandler;
    private SettingsViewModel _settingsViewModel;

    public ICollection<IFamilyTypeViewModel> SelectedItems { get; set; } = new Collection<IFamilyTypeViewModel>();

    public ChangeElementInterface(AsyncEventHandler eventHandler)
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
        await _eventHandler.RaiseAsync( app =>
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

                            if (_settingsViewModel._isSelectedWidth)
                            {
                                double widthIncrement = UnitUtils.ConvertToInternalUnits(_settingsViewModel._widthIncrement, DisplayUnitType.DUT_MILLIMETERS);
                                widthParam.Set(widthIncrement);
                            }
                            else
                            {
                                widthParam.Set(widthDefault);
                            }


                            if (_settingsViewModel._isSelectedHeight)
                            {
                                double heightIncrement = UnitUtils.ConvertToInternalUnits(_settingsViewModel._heightIncrement, DisplayUnitType.DUT_MILLIMETERS);
                                heightParam.Set(heightIncrement);
                            } else {
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


