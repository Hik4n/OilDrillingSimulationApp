using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace OilDrillingSimulationApp.Converters
{
    public class ObjectIsNotNullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}