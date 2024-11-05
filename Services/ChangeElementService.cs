using Autodesk.Revit.DB;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.Interfaces;
using RevitTest.Model;
using RevitTest.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace RevitTest.Services
{
    public class ChangeElementService : IChangeElement
    {
        private readonly AsyncEventHandler _eventHandler;

        public ICollection<IFamilyTypeViewModel> SelectedItems { get; set; } = new Collection<IFamilyTypeViewModel>();

        public ChangeElementService(AsyncEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }
        public async Task ExecuteChangeAsync(AppSettings settings)
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
                            ProcessWindowElement(doc, item, settings);
                        }
                    }

                    trans.Commit();
                }

                NotifyUser(SelectedItems.Count);
            });
        }
        private void ProcessWindowElement(Document doc, IFamilyTypeViewModel item, AppSettings settings)
        {
            var collector = doc.GetElement(item.Id);
            var widthParam = collector.get_Parameter(BuiltInParameter.WINDOW_WIDTH);
            var heightParam = collector.get_Parameter(BuiltInParameter.WINDOW_HEIGHT);

            if (widthParam != null && heightParam != null)
            {
                AdjustParameter(widthParam, settings.IsSelectedWidth, settings.WidthIncrement);
                AdjustParameter(heightParam, settings.IsSelectedHeight, settings.HeightIncrement);
            }
        }
        private void AdjustParameter(Parameter parameter, bool isSelected, double incrementValue)
        {
            double defaultValue = parameter.AsDouble();

            if (isSelected)
            {
                double increment = UnitUtils.ConvertToInternalUnits(incrementValue, DisplayUnitType.DUT_MILLIMETERS);
                parameter.Set(increment);
            }
            else
            {
                parameter.Set(defaultValue);
            }
        }

        private void NotifyUser(int itemCount)
        {
            MessageBox.Show($"Изменение размеров для {itemCount} окон завершено.");
        }
    }
}
