﻿namespace MoneyMap.Converters
    {
    public class BalanceColorConverter : IValueConverter
        {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            if (value is decimal balance)
                {
                return balance >= 0 ? Colors.Green : Colors.Red;
                }

            return Colors.Black; // Default fallback color
            }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
            throw new NotImplementedException();
            }
        }
    }