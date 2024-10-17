using Autodesk.Revit.DB;
using RevitTest.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System;
using Autodesk.Revit.UI;


namespace RevitTest.ViewModel { 
public partial class MainViewModel : ViewModelBase
{   private ObservableCollection<IFamilyViewModel> _revitElements = new ObservableCollection<IFamilyViewModel>();
    public ObservableCollection<IFamilyViewModel> RevitElements
    {
        get => _revitElements;
        set
        {
            _revitElements = value;
            OnPropertyChanged(nameof(RevitElements));
        }
    }
    public void AddRevitElement(IFamilyViewModel element)
    {
        RevitElements.Add(element);
        OnPropertyChanged(nameof(RevitElements));
    }

    public void ClearRevitElements()
    {
        RevitElements.Clear();
        OnPropertyChanged(nameof(RevitElements));
    }

        private ObservableCollection<IFamilyViewModel> _selectedItem;
        public ObservableCollection<IFamilyViewModel> SelectedItem
        {
            get { return _selectedItem; 
            
            
            }
            set { _selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }
    }
    }
