using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitTest.View;
using RevitTest.ViewModel;

namespace RevitTest.ComponentRevit
{
    [Transaction(TransactionMode.Manual)]
    internal class EntryCommand : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

                var uiapp = commandData.Application;
                var uidoc = uiapp.ActiveUIDocument;
                var doc = uidoc.Document;

                var mainViewModel = new MainViewModel();
                var mainWindow = new Main
                {
                    DataContext = mainViewModel
                };

                mainWindow.Show();

                

            return Result.Succeeded;
        }
    }
}
