using RevitTest.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;


namespace RevitTest.Converter
{
    public class SelectedItemsCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<IFamilyTypeViewModel> selectedItem)
            {
                return selectedItem;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

