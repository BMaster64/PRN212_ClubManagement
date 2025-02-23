﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace PRN212_Project.Converters
{
    public class UserTypeToRoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int userType)
            {
                return userType switch
                {
                    1 => "Chủ nhiệm",
                    2 => "Phó chủ nhiệm",
                    3 => "Trưởng ban",
                    4 => "Thành viên",
                    _ => "Unknown"
                };
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}