using System;
using System.Globalization;
using System.Windows.Media.Imaging;
using Tamarillo.Classes.Converters.Base;
using Tamarillo.Classes.Helpers;
using static System.Windows.Application;

namespace Tamarillo.Classes.Converters {

    public class TypeToImageConverter : Converter<TypeToImageConverter> {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (!(value is Game source)) {
                return null;
            }

            switch (source) {
                case Game.H1:  return (BitmapImage)Current.FindResource("UI_IconH1");
                case Game.H2:  return (BitmapImage)Current.FindResource("UI_IconH2");
                case Game.H2A: return (BitmapImage)Current.FindResource("UI_IconH2A");
                case Game.H3:  return (BitmapImage)Current.FindResource("UI_IconH3");
                case Game.H3O: return (BitmapImage)Current.FindResource("UI_IconH3O");
                case Game.HR:  return (BitmapImage)Current.FindResource("UI_IconHR");
                case Game.H4:  return (BitmapImage)Current.FindResource("UI_IconH4");
                default:
                    throw new ArgumentOutOfRangeException(nameof(source));
            }

        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    }

}