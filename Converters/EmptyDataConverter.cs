namespace MoneyMap.Converters
    {
    public class EmptyDataConverter : IValueConverter
        {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            if (value is int count && count == 0)
                {
                return "No transactions yet. Start adding transactions!";
                }
            return value; // Show actual data if not empty
            }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
            throw new NotImplementedException();
            }
        }
    }
