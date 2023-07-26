using System;
using System.Globalization;
using System.Windows;
using Tamarillo.Classes.Converters.Base;

namespace Tamarillo.Classes.Converters {

    public class StringToVisibilityConverter : Converter<StringToVisibilityConverter> {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (!(value is string)) {
                return Visibility.Visible;
            }

            return string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;

        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    }

}