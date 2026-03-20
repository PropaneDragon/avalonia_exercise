using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

using SystemColor = System.Drawing.Color;

namespace AvaloniaExercise.Converters
{
    public class ColourToAvaloniaColourConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not SystemColor colour) return null;

            return Color.FromUInt32((uint)colour.ToArgb());
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
