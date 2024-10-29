using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.ComponentRevit.Handlers;
using RevitTest.View;
using RevitTest.ViewModel;

using System.Windows.Threading;

namespace RevitTest
{
    [Transaction(TransactionMode.Manual)]
    internal class EntryCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var serviceCollection = new ServiceCollection();

            var asyncEventHandler = new AsyncEventHandler();
            serviceCollection.AddSingleton(asyncEventHandler);
            serviceCollection.AddSingleton<IPickElementService, PickElementService>();
            serviceCollection.AddSingleton<IChangeElementService, ChangeElementService>();
            serviceCollection.AddSingleton<MainViewModel>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
          
            var thread = new Thread(() => ThreadEntry(mainViewModel));
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();

            return Result.Succeeded;
        }


        private void ThreadEntry(MainViewModel mainViewModel)
        {
            var mainWindow = new Main
            {
                DataContext = mainViewModel
            };
            mainViewModel.UiDispatcher = mainWindow.Dispatcher;

            mainWindow.Closed += (s, e) => Dispatcher.CurrentDispatcher.InvokeShutdown();

            mainWindow.Show();
            Dispatcher.Run();
        }
    }
}
