using System;
using System.Collections.Generic;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitTest.View;
using RevitTest.ViewModel;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitTest.ComponentRevit
{
    [Transaction(TransactionMode.Manual)]
    internal class RevitAPI : IExternalCommand
    {
        private ExternalCommandData commandData; 

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            this.commandData = commandData;
            try
            {
                var uiapp = commandData.Application;
                var uidoc = uiapp.ActiveUIDocument;
                var doc = uidoc.Document;

                if (uidoc == null || doc == null)
                {
                    message = "Не удалось получить доступ к текущему документу.";
                    return Result.Failed;
                }

                var mainViewModel = new MainViewModel(uidoc); 
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
                            e => e is FamilyInstance fi &&
                                 fi.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows
                        ));

                        var windows = new List<FamilyInstance>();
                        foreach (var reference in references)
                        {
                            var window = doc.GetElement(reference) as FamilyInstance;
                            if (window != null) windows.Add(window);
                        }

                        mainViewModel.ClearRevitElements();
                        foreach (var window in windows)
                        {
                            var viewModel = new WindowFamilyViewModel(window.Name, window.Id, false);
                            mainViewModel.AddRevitElement(viewModel);
                        }

                        if (windows.Count > 0)
                        {
                            mainViewModel.ChangeWindowSizeCommand.Execute(null); 
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        message = "Произошла ошибка: " + ex.Message;
                        return Result.Failed;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Result.Succeeded;
        }
    }
}
