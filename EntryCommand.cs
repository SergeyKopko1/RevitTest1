using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.Services;
using RevitTest.Interfaces;
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

            serviceCollection.AddSingleton<AsyncEventHandler>();
            serviceCollection.AddSingleton<IPickElementService, PickElementInterface>();
            serviceCollection.AddSingleton<IChangeElementService, ChangeElementInterface>();
            serviceCollection.AddSingleton<MainViewModel>();
            serviceCollection.AddSingleton<SettingsViewModel>();


            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();


            var thread_one = new Thread(() => ThreadEntry(mainViewModel));
            thread_one.SetApartmentState(ApartmentState.STA);
            thread_one.IsBackground = true;
            thread_one.Start();



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
