using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RevitTest.Model
{
    internal class RevitElementModel
    {
       public string Name { get; set; }
       public ElementId Id { get; set; }
    }
}