using Autodesk.Revit.DB;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Autodesk.Revit.UI;
using System.Windows;
using System.Collections.Generic;


namespace RevitTest.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {


        private readonly UIDocument _uidoc;

        public MainViewModel(UIDocument uidoc)
        {
            _uidoc = uidoc;
        }

        private ObservableCollection<IFamilyViewModel> _revitElements = new ObservableCollection<IFamilyViewModel>();
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

        private ObservableCollection<IFamilyViewModel> _selectedItems = new ObservableCollection<IFamilyViewModel>();
        public ObservableCollection<IFamilyViewModel> SelectedItems
        {
            get => _selectedItems;
            set
            {
                _selectedItems = value;

                OnPropertyChanged(nameof(SelectedItems));
                
            }
        }

        public ICommand ChangeWindowSizeCommand => new RelayCommand(ChangeWindowSize);

        private void ChangeWindowSize()
        {
            var selectedItems = SelectedItems.ToList();

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов для изменения размера.");
                return;
            }
            using (var transaction = new Transaction(_uidoc.Document, "Изменение размеров окон"))
            {
                transaction.Start();
                foreach (var item in selectedItems)
                {
                    if (item is WindowFamilyViewModel windowViewModel)

                    {
                        var window = _uidoc.Document.GetElement(item.Id) as FamilyInstance;



                        if (window == null)
                        {
                            MessageBox.Show($"Окно с ID {item.Id} не найдено.");
                            continue;
                        }

                        var widthWindow = window.get_Parameter(BuiltInParameter.WINDOW_WIDTH);
                        var heightWindow = window.get_Parameter(BuiltInParameter.WINDOW_HEIGHT);

                        if (widthWindow == null || heightWindow == null)
                        {
                            continue;
                        }
                        double newHeight = heightWindow.AsDouble() * 2;
                        double newWidth = widthWindow.AsDouble() * 2;

                        widthWindow.SetValueString(newWidth.ToString());
                        heightWindow.SetValueString(newHeight.ToString());
                    }
                }

                transaction.Commit();
            }
        }



    }
}
