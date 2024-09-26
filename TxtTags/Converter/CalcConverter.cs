using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace TxtTags.Converter
{
    public class PlusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (values == null)
            {
                return Binding.DoNothing;
            }
            if (values.Length < 2)
            {
                return System.Convert.ChangeType(values[0], targetType);
            }
            var l = System.Convert.ToDecimal(values[0]);
            var r = System.Convert.ToDecimal(values[1]);

            return System.Convert.ChangeType(l + r, targetType);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MinusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (values == null)
            {
                return Binding.DoNothing;
            }
            if (values.Length < 2)
            {
                return System.Convert.ChangeType(values[0], targetType);
            }
            var l = System.Convert.ToDecimal(values[0]);
            var r = System.Convert.ToDecimal(values[1]);

            return System.Convert.ChangeType(l - r, targetType);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MultipliedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (values == null)
            {
                return Binding.DoNothing;
            }
            if (values.Length < 2)
            {
                return System.Convert.ChangeType(values[0], targetType);
            }
            var l = System.Convert.ToDecimal(values[0]);
            var r = System.Convert.ToDecimal(values[1]);

            return System.Convert.ChangeType(l * r, targetType);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DividedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (values == null)
            {
                return Binding.DoNothing;
            }
            if (values.Length < 2)
            {
                return System.Convert.ChangeType(values[0], targetType);
            }
            var l = System.Convert.ToDecimal(values[0]);
            var r = System.Convert.ToDecimal(values[1]);

            return System.Convert.ChangeType(l / r, targetType);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var t = System.Convert.ToDecimal(value);
            var p = System.Convert.ToDecimal(parameter.ToString());
            var v = t * (p / 100);
            return System.Convert.ChangeType(v, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class GreaterShowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var t = System.Convert.ToDecimal(value);
            var p = System.Convert.ToDecimal(parameter.ToString());
            return t > p ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class GreaterHideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var t = System.Convert.ToDecimal(value);
            var p = System.Convert.ToDecimal(parameter.ToString());
            return t > p ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class LessShowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var t = System.Convert.ToDecimal(value);
            var p = System.Convert.ToDecimal(parameter.ToString());
            return t < p ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class LessHideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var t = System.Convert.ToDecimal(value);
            var p = System.Convert.ToDecimal(parameter.ToString());
            return t < p ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class CalcDetect
    {
        public static int Compare(object x, object y)
        {
            if (x == null || x == DependencyProperty.UnsetValue)
            {
                return -1;
            }
            if (y == null || y == DependencyProperty.UnsetValue)
            {
                return 1;
            }
            var l = System.Convert.ToDecimal(x);
            var r = System.Convert.ToDecimal(y);
            return l.CompareTo(r);
        }
    }
    public class MultiGreaterShowMConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2)
                {
                    return Binding.DoNothing;
                }
                int compare = CalcDetect.Compare(values[0], values[1]);
                return compare > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MultiGreaterHideMConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2)
                {
                    return Binding.DoNothing;
                }
                int compare = CalcDetect.Compare(values[0], values[1]);
                return compare > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            catch
            {
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MultiLessShowMConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2)
                {
                    return Binding.DoNothing;
                }
                int compare = CalcDetect.Compare(values[0], values[1]);
                return compare < 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MultiLessHideMConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2)
                {
                    return Binding.DoNothing;
                }
                int compare = CalcDetect.Compare(values[0], values[1]);
                return compare < 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            catch
            {
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class GreaterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var t = System.Convert.ToDecimal(value);
            var p = System.Convert.ToDecimal(parameter.ToString());
            return t > p;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class LessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var t = System.Convert.ToDecimal(value);
            var p = System.Convert.ToDecimal(parameter.ToString());
            return t < p;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class NumEqualsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var t = System.Convert.ToDecimal(value);
            var p = System.Convert.ToDecimal(parameter);
            return t == p;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class NumEqualsShowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var t = System.Convert.ToDecimal(value);
            var p = System.Convert.ToDecimal(parameter);
            return t == p ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class MultiNumEqualsShowConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2)
                {
                    return Binding.DoNothing;
                }
                int compare = CalcDetect.Compare(values[0], values[1]);
                return compare == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
