using Autodesk.Revit.UI;
using Nice3point.Revit.Extensions;
using Nice3point.Revit.Toolkit.External;
using RevitTest.ComponentRevit;

namespace RevitTest;

public class Application : ExternalApplication
{
    /// <summary>
    /// Этот метод вызывается при запуске Revit.
    /// </summary>
    public override void OnStartup()
    {
        var panel = Application.CreatePanel("Manage", tabName: "Eneca");
        var showButton = panel.AddPushButton<EntryCommand>("Clashes\nManager");

        // Устанавливаем иконку кнопки
        //showButton.SetLargeImage("/Resources/icons/revit.png");

        // Устанавливаем описание и подсказку напрямую
        showButton.LongDescription = "This button opens the Clash Manager for reviewing and managing model clashes.";
        showButton.ToolTip = "Open Clash Manager";
    }

    /// <summary>
    /// Этот метод вызывается при завершении работы Revit.
    /// </summary>
    public override void OnShutdown()
    {
        // Здесь можно добавить код для очистки или сохранения данных перед выходом из Revit.
    }
}