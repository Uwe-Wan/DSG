using System;
using System.Globalization;
using System.Windows.Data;

namespace DSG.Presentation.Converter
{
    public class ObjectSelectedToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "This is not needed";
        }
    }
}
