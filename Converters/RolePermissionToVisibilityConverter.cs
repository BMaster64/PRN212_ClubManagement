using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PRN212_Project.Converters
{
    public class RolePermissionToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is int userRoleId && values[1] is int currentUserRoleId)
            {
                // Current user can only modify users with higher RoleId (lower rank)
                // e.g., Admin (RoleId=1) can modify Club Leader (RoleId=2)
                if ((userRoleId >= currentUserRoleId && currentUserRoleId != 4) || currentUserRoleId == 5)
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}