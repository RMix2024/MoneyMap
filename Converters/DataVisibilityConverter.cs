namespace MoneyMap.Converters
    {
    public class DataVisibilityConverter : IValueConverter
        {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            if (value is int count && count > 0)
                return true; // Show chart if data exists

            return false; // Hide chart if empty
            }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
            throw new NotImplementedException();
            }
        }
    }
