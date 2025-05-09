using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace OilDrillingSimulationApp.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string colors)
            {
                var colorParts = colors.Split(';');
                if (colorParts.Length == 2)
                {
                    return boolValue ? Brush.Parse(colorParts[0]) : Brush.Parse(colorParts[1]);
                }
            }
            return Brushes.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}