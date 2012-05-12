

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ColorScale;

namespace MovieManager.APP.Common
{
    class Int2ColorConverter : IValueConverter
    {
        private static readonly ColorPercent[] PERCENT_COLORS = new[]
                                {
                                    new ColorPercent{Percent = 0, Red = 255, Green = 0, Blue = 0}, 
                                    new ColorPercent{Percent = 0.5, Red = 255, Green = 255, Blue = 0}, 
                                    new ColorPercent{Percent = 1, Red = 0, Green = 255, Blue = 0}
                                };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Brush) && value is int)
            {
                int Percent = (int)value;
                if (Percent < 0 || Percent > 100)
                {
                    return null;
                }
                return new SolidColorBrush(Percent2Color(Percent));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static Color Percent2Color(double percent)
        {
            double Pct = percent / 100.0;
            for (var I = 0; I < PERCENT_COLORS.Length; I++)
            {
                if (Pct <= PERCENT_COLORS[I].Percent)
                {
                    ColorPercent Lower = I-1 < 0 ? new ColorPercent{Percent = 0.1, Red = 0, Green = 0, Blue = 0} : PERCENT_COLORS[I - 1];
                    var Upper = PERCENT_COLORS[I];
                    var Range = Upper.Percent - Lower.Percent;
                    var RangePct = (Pct - Lower.Percent) / Range;
                    var PctLower = 1 - RangePct;
                    var PctUpper = RangePct;
                    byte Red = (byte)Math.Round(Lower.Red * PctLower + Upper.Red * PctUpper);
                    byte Green = (byte)Math.Round(Lower.Green * PctLower + Upper.Green * PctUpper);
                    byte Blue = (byte)Math.Round(Lower.Blue * PctLower + Upper.Blue * PctUpper);
                    return Color.FromRgb(Red, Green, Blue);
                }
            }
            return Colors.DarkGray;
        }
    }
}
