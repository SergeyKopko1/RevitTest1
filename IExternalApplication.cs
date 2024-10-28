using Autodesk.Revit.UI;
using Nice3point.Revit.Extensions;
using RevitTest.ComponentRevit;


namespace RevitTest
{
    public class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            var panel = application.CreatePanel("Manage", tabName: "Eneca");
            var showButton = panel.AddPushButton<EntryCommand>("Clashes\nManager");

            // Устанавливаем иконку кнопки
            showButton.SetLargeImage("uri://ClashesManager.Revit;component/Resources/Icons/RibbonIcon32.png");

            // Устанавливаем описание и подсказку напрямую
            showButton.LongDescription = "This button opens the Clash Manager for reviewing and managing model clashes.";
            showButton.ToolTip = "Open Clash Manager";

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}