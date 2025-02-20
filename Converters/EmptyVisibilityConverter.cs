namespace MoneyMap.Converters
    {
    public class EmptyVisibilityConverter : IValueConverter
        {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            if (value is int count && count == 0)
                return true; // Show placeholder

            return false; // Hide placeholder if data exists
            }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
            throw new NotImplementedException();
            }
        }
    }
