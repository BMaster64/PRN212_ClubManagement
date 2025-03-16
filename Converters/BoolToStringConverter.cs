using System;
using System.Globalization;
using System.Windows.Data;

namespace PRN212_Project.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string options)
            {
                string[] parts = options.Split('|');
                if (boolValue && parts.Length > 0)
                    return parts[0];
                else if (!boolValue && parts.Length > 1)
                    return parts[1];
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
