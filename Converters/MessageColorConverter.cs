using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PRN212_Project.Converters
{
    public class MessageColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentUser = (App.Current as App)?.GetCurrentUser();
            if (currentUser == null) return new SolidColorBrush(Colors.LightGray);

            string senderId = value as string;
            return senderId == currentUser.StudentId
                ? new SolidColorBrush(Color.FromRgb(143, 217, 255))  // Light blue for current user
                : new SolidColorBrush(Color.FromRgb(225, 225, 225)); // Light gray for other users
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
