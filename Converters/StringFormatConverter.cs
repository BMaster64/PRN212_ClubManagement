using System.Globalization;
using System.Windows.Data;

namespace PRN212_Project.Converters
{
    // Converter for formatting strings based on a boolean value
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string format)
            {
                var options = format.Split(',');
                return boolValue ? options[0] : options[1];
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter for selecting command based on a boolean value
    public class CommandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string commandString)
            {
                var commands = commandString.Split(',');
                return boolValue ? (object)Activator.CreateInstance(Type.GetType(commands[0])) : (object)Activator.CreateInstance(Type.GetType(commands[1]));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}