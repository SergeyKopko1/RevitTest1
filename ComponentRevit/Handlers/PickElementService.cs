﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External.Handlers;

using RevitTest.ComponentRevit.Extensions.ExtenstionSelections;
using RevitTest.ViewModel;


namespace RevitTest.ComponentRevit.Handlers;

public class PickElementService : IPickElementService
{
    private readonly MainViewModel _mainViewModel;
    private readonly AsyncEventHandler _eventHandler;
    private MainViewModel mainViewModel;

    public PickElementService(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
    }

    public PickElementService(MainViewModel viewModel, AsyncEventHandler eventHandler)
    {
        _mainViewModel = viewModel;
        _eventHandler = eventHandler;
    }

    public async Task ExecutePickAsync()
    {
        await _eventHandler.RaiseAsync(async app =>
        {
            var uidoc = app.ActiveUIDocument;
            var doc = uidoc.Document;

            try
            {
                var references = uidoc.Selection.PickObjects(ObjectType.Element, new SelectionsFilter(
                    e => e is FamilyInstance fi &&
                         fi.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows
                ));

                var selectedElementIds = references.Select(r => r.ElementId).ToList();

             
                _mainViewModel.ClearRevitElements();

         
                HashSet<ElementId> uniqueTypes = new();
                foreach (var elementId in selectedElementIds)
                {
                    var windowFromDoc = doc.GetElement(elementId);
                    var windowTypeFromDoc = windowFromDoc.GetTypeId();
                    uniqueTypes.Add(windowTypeFromDoc);
                }


                foreach (var uniqueTypeId in uniqueTypes)
                {
                    var windowTypeElement = doc.GetElement(uniqueTypeId) as FamilySymbol;
                    if (windowTypeElement != null)
                    {
                        string name = windowTypeElement.Name;
                        var viewModel = new WindowFamilyTypeViewModel(name, uniqueTypeId);
                        _mainViewModel.AddRevitElement(viewModel);
                    }
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
      
            }
        });
    }

    public string GetName()
    {
        return "Выбор элементов";
    }
}