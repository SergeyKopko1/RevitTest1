using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

namespace RevitTest.ComponentRevit.Extensions.ExtenstionSelections
{
    public class SelectionsFilter : ISelectionFilter
    {
        private readonly Predicate<Element> _criteria;

        public SelectionsFilter(Predicate<Element> criteria)
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
}