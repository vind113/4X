﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace _4XGame.ViewModel.Converters {
    class SelectCelestialBodyConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            Tuple<object, object> tuple = new Tuple<object, object>(values[0], values[1]);
            return (object)tuple;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotSupportedException();
        }
    }
}
