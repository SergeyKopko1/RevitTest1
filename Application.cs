using Nice3point.Revit.Extensions;
using Nice3point.Revit.Toolkit.External;


namespace RevitTest;

public class Application : ExternalApplication
{
    public override void OnStartup()
    {
        var panel = Application.CreatePanel("Test", tabName: "Demo");
        var showButton = panel.AddPushButton<EntryCommand>("Test\nManager");


        showButton.SetLargeImage("pack://application:,,,/RevitTest;component/Resources/Icons/revit.png");

        showButton.LongDescription = "This button opens the Clash Manager for reviewing and managing model clashes.";
        showButton.ToolTip = "Open Clash Manager";
    }


    public override void OnShutdown()
    {

    }
}