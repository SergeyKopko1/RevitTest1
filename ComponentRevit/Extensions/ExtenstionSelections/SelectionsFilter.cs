using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

public class SelectionsFilter : ISelectionFilter
{
    private readonly Func<Element, bool> _criteria;

    public SelectionsFilter(Func<Element, bool> criteria)
    {
        _criteria = criteria;
    }

    public bool AllowElement(Element elem)
    {
        return _criteria(elem);
    }

    public bool AllowReference(Reference reference, XYZ position)
    {
        return false;
    }
}
