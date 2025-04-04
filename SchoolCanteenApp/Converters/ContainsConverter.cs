using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace SchoolCanteenApp.Converters
{
    public class ContainsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id && parameter is List<int> selectedIds)
                return selectedIds.Contains(id);

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}