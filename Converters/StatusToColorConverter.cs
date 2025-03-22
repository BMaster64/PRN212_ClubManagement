using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PRN212_Project.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status.ToLower())
                {
                    case "pending":
                        return new SolidColorBrush(Color.FromRgb(255, 165, 0)); // Orange
                    case "read":
                        return new SolidColorBrush(Color.FromRgb(0, 128, 0));   // Green
                    default:
                        return new SolidColorBrush(Color.FromRgb(128, 128, 128)); // Gray
                }
            }

            return new SolidColorBrush(Color.FromRgb(128, 128, 128)); // Default: Gray
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}