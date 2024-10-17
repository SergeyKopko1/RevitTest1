using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace RevitTest.ViewModel
{
    public class IFamilyViewModel : ViewModelBase
    {
        string Name { get; }
        ElementId Id { get; }

    }
}