using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace RevitTest.ViewModel
{
    public interface IFamilyViewModel
    {
        string Name { get; }
        ElementId Id { get; }
        bool IsSelected { get; set; }
    }
}