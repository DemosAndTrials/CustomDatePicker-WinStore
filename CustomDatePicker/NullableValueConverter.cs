using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace CustomDatePicker
{
    public class NullableValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? string.Empty : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string s = value.ToString();

            DateTime result;
            if (!string.IsNullOrWhiteSpace(s) && DateTime.TryParse(s, out result))
            {
                return result;
            }

            return null;
        }
    }
}
