using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.Interfaces;
using RevitTest.Services;
using RevitTest.View;
using RevitTest.ViewModel;
using System;
using System.Threading;
using System.Windows.Threading;

namespace RevitTest
{
    [Transaction(TransactionMode.Manual)]
    internal partial class EntryCommand : IExternalCommand, IDisposable
    {
        private MainView _mainWindow;
        private SettingsView _settingsWindow;
        private readonly IServiceProvider _serviceProvider;

        public EntryCommand()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<AsyncEventHandler>();
            serviceCollection.AddSingleton<IPickElementInterface, PickElementService>();
            serviceCollection.AddSingleton<IChangeElementInterface, ChangeElementService>();
            serviceCollection.AddSingleton<MainViewModel>();
            serviceCollection.AddSingleton<SettingsView>();
            serviceCollection.AddSingleton<SettingsViewModel>();


            _serviceProvider = serviceCollection.BuildServiceProvider();
            _serviceProvider.GetRequiredService<AsyncEventHandler>();
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var threadOne = new Thread(() => ThreadEntry(_serviceProvider));
            threadOne.SetApartmentState(ApartmentState.STA);
            threadOne.IsBackground = true;
            threadOne.Start();

            return Result.Succeeded;
        }

        private void ThreadEntry(IServiceProvider serviceProvider)
        {
            var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
 

            _mainWindow = new MainView
            {
                DataContext = mainViewModel
            };
            mainViewModel.UiDispatcher = _mainWindow.Dispatcher;

            _mainWindow.Closed += OnMainWindowClosed;
            _mainWindow.Show();
            Dispatcher.Run();
        }

        private void OnMainWindowClosed(object sender, EventArgs e)
        {
            if (_mainWindow != null)
            {
                _mainWindow.Closed -= OnMainWindowClosed;
            }
            Dispatcher.CurrentDispatcher.InvokeShutdown();
        }

        public void Dispose()
        {
            if (_mainWindow != null)
            {
                _mainWindow.Closed -= OnMainWindowClosed;
                _mainWindow = null;
            }
        }
    }
}
