using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace _4XGame.ViewModel.Converters {
    public class ColonizationModeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value?.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }
}
