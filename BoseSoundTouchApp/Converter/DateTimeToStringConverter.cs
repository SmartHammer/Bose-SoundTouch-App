using System;
using Windows.UI.Xaml.Data;

namespace BoseSoundTouchApp.Converter
{
    class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (! (value is DateTime))
            {
                throw new ArgumentException();
            }
            if (targetType.Name != typeof(string).Name)
            {
                throw new ArgumentException();
            }
            if (parameter.ToString() is "Date")
            {
                DateTime dateTime = (DateTime)value;
                return dateTime.ToString("dd.MM.yyyy");
            }
            else if (parameter.ToString() is "Time")
            {
                DateTime dateTime = (DateTime)value;
                return dateTime.ToString("HH:mm");
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
