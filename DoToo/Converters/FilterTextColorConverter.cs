namespace DoToo.Converters;

using System;
using System.Globalization;

internal class FilterTextColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => 
        (bool)value! ? Colors.White : Colors.Black;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => 
        null!;
}