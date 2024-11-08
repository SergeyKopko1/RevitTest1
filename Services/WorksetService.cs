using Autodesk.Revit.DB;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitTest.Interfaces;
using System.Collections.ObjectModel;


namespace RevitTest.Services
{
    public class WorksetService : IWorkset
    {
        private readonly AsyncEventHandler _eventHandler;

        public WorksetService(AsyncEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }


        public async Task AssignElementsToWorksetAsync(ObservableCollection<IFamilyTypeViewModel> elements)
        {
            await _eventHandler.RaiseAsync(app =>
            {
                var uidoc = app.ActiveUIDocument;
                var doc = uidoc.Document;

                var windowWorkset = new FilteredWorksetCollector(doc)
                    .OfKind(WorksetKind.UserWorkset)
                    .FirstOrDefault(w => w.Name == "Окна") ?? CreateWorkset(doc, "Окна");

                var doorWorkset = new FilteredWorksetCollector(doc)
                    .OfKind(WorksetKind.UserWorkset)
                    .FirstOrDefault(w => w.Name == "Двери") ?? CreateWorkset(doc, "Двери");

                using (Transaction trans = new Transaction(doc, "Assign elements to respective worksets"))
                {
                    trans.Start();

                    foreach (var element in elements)
                    {
                        var familyInstances = new FilteredElementCollector(doc)
                            .OfClass(typeof(FamilyInstance))
                            .Where(x => x.Name == element.Name)
                            .ToList();

                        foreach (var familyInstance in familyInstances)
                        {
                            var targetWorkset = (element.CategoryElement == "Окна" || element.CategoryElement == "Windows") ? windowWorkset : doorWorkset;

                            var worksetParam = familyInstance?.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);
                            if (worksetParam != null && !worksetParam.IsReadOnly)
                            {
                                worksetParam.Set(targetWorkset.Id.IntegerValue);
                            }
                        }
                    }

                    trans.Commit();
                }
            });
        }
        private Workset CreateWorkset(Document doc, string worksetName)
        {
            using (Transaction trans = new Transaction(doc, $"Create {worksetName} workset"))
            {
                trans.Start();
                var workset = Workset.Create(doc, worksetName);
                trans.Commit();
                return workset;
            }
        }
    }
}
