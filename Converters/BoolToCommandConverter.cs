using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace PRN212_Project.Converters
{
    public class BoolToCommandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string commandNames)
            {
                string[] commands = commandNames.Split('|');
                string propertyName = boolValue ? commands[0] : commands[1];

                // Get the DataContext from the current application's main window
                var dataContext = System.Windows.Application.Current.MainWindow.DataContext;
                if (dataContext != null)
                {
                    try
                    {
                        // Try to get the command from DataContext
                        var property = dataContext.GetType().GetProperty(propertyName);
                        if (property != null)
                        {
                            return property.GetValue(dataContext);
                        }
                    }
                    catch { }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}