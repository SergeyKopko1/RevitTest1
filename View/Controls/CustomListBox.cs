using RevitTest.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace RevitTest.View.Controls
{
    public class CustomListBox : ListBox
    {
        public static readonly DependencyProperty BoundSelectedItemsProperty =
            DependencyProperty.Register("BoundSelectedItems", typeof(ObservableCollection<IFamilyViewModel>), typeof(CustomListBox), new PropertyMetadata(new ObservableCollection<IFamilyViewModel>()));

        public ObservableCollection<IFamilyViewModel> BoundSelectedItems
        {
            get => (ObservableCollection<IFamilyViewModel>)GetValue(BoundSelectedItemsProperty);
            set => SetValue(BoundSelectedItemsProperty, value);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            foreach (var item in e.RemovedItems)
            {
                BoundSelectedItems.Remove(item as IFamilyViewModel);
            }
            foreach (var item in e.AddedItems)
            {
                BoundSelectedItems.Add(item as IFamilyViewModel);
            }
        }
    }
}
