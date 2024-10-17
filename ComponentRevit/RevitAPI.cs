using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using RevitTest.View;
using RevitTest.ViewModel;
using System.Windows;
using RevitTest.View.Controls;
using System;

namespace RevitTest.ComponentRevit
{
    [Transaction(TransactionMode.Manual)]
    internal class RevitAPI : IExternalCommand
    {
        private MainViewModel mainViewModel;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            if (uidoc == null || doc == null)
            {
                message = "Не удалось получить доступ к текущему документу.";
                return Result.Failed;
            }

            mainViewModel = new MainViewModel();

            var mainWindow = new Main
            {
                DataContext = mainViewModel
            };

            mainWindow.Show();

            while (mainWindow.IsVisible)
            {
                try
                {
                    var references = uidoc.Selection.PickObjects(ObjectType.Element, new SelectionsFilter(
                        e => e is FamilyInstance fi && fi.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows
                    ));

                    var windows = new List<FamilyInstance>();

                    foreach (var reference in references)
                    {
                        var window = doc.GetElement(reference) as FamilyInstance;
                        if (window != null)
                        {
                            windows.Add(window);
                        }
                    }

                    mainViewModel.ClearRevitElements();
                    foreach (var window in windows)
                    {
                        var viewModel = new WindowFamilyViewModel(window.Name, window.Id);
                        mainViewModel.AddRevitElement(viewModel);
                    }
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    message = "Произошла ошибка: " + ex.Message;
                    return Result.Failed;
                }
            }

            return Result.Succeeded;
        }

    }
}
