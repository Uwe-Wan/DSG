using DSG.BusinessEntities.CardTypes;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DSG.Presentation.Converter
{
    public class CardTypeEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.GetName(value.GetType(), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (CardTypeEnum)Enum.Parse(typeof(CardTypeEnum), (string)value);
        }
    }
}
