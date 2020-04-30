using DSG.BusinessEntities.CardSubTypes;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DSG.Presentation.Converter
{
    public class CardSubTypeEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.GetName(value.GetType(), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (CardSubTypeEnum)Enum.Parse(typeof(CardSubTypeEnum), (string)value);
        }
    }
}
