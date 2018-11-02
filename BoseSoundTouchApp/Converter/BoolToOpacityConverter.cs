using System;
using System.Globalization;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace BoseSoundTouchApp.Converter
{
    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((value is bool) && (targetType == typeof(double)))
            {
                double resultForTrue = 1.0;
                double resultForFalse = 0.0;
                if (!string.IsNullOrEmpty(parameter.ToString()))
                {
                    var parameters = parameter.ToString().Split(';');
                    if (parameters.Length > 0 && ! string.IsNullOrEmpty(parameters[0]))
                    {
                        double.TryParse(parameters[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture.NumberFormat, out resultForTrue);
                    }
                    if (parameters.Length > 1 && !string.IsNullOrEmpty(parameters[1]))
                    {
                        double.TryParse(parameters[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture.NumberFormat, out resultForFalse);
                    }
                }

                return (bool)value ? resultForTrue : resultForFalse;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
