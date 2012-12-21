using System;
using System.Windows;
using System.Windows.Data;

namespace Tmc.WinUI.Application.Converters
{
    public class EnumBooleanConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string ParameterString = parameter as string;
            if (ParameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object ParameterValue = Enum.Parse(value.GetType(), ParameterString);

            return ParameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string ParameterString = parameter as string;
            if (ParameterString == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, ParameterString);
        }
        #endregion
    }
}
