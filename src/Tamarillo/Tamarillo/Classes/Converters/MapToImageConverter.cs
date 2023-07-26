using System;
using System.Globalization;
using System.Windows.Media.Imaging;
using Tamarillo.Classes.Converters.Base;
using static System.Windows.Application;

namespace Tamarillo.Classes.Converters {

    public class MapToImageConverter : Converter<MapToImageConverter> {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                if (value is string == false)
                    throw new Exception("");
                return (BitmapImage)Current.FindResource(value);
            }
            catch {
                return (BitmapImage)Current.FindResource("map_unknown");
            }
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    }

}