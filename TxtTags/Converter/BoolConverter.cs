using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Collections;

namespace TxtTags.Converter
{
    public class BoolDetect
    {
        public static bool IsTrue(object value)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return false;
            }
            if (typeof(bool).Equals(value.GetType()))
            {
                return (bool)value;
            }
            if (typeof(string).Equals(value.GetType()))
            {
                return !string.IsNullOrWhiteSpace(value.ToString());
            }
            if (typeof(byte).Equals(value.GetType()) || 
                typeof(short).Equals(value.GetType()) || 
                typeof(int).Equals(value.GetType()) || 
                typeof(long).Equals(value.GetType()) ||
                typeof(ushort).Equals(value.GetType()) ||
                typeof(uint).Equals(value.GetType()) ||
                typeof(ulong).Equals(value.GetType()) ||
                typeof(float).Equals(value.GetType()) || 
                typeof(double).Equals(value.GetType()))
            {
                return Convert.ToDecimal(value) != 0m;
            }
            if(typeof(ICollection).Equals(value.GetType()))
            {
                return ((ICollection)value).Count > 0;
            }
            return true;
        }
    }
    public class HideWhenFalseConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return BoolDetect.IsTrue(value)? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ShowWhenFalseConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return BoolDetect.IsTrue(value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsFalseConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !BoolDetect.IsTrue(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsTrueConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return BoolDetect.IsTrue(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
