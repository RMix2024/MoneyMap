﻿namespace MoneyMap.Converters
    {
    public class TransactionTypeColorConverter : IValueConverter
        {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            if (value is TransactionType transactionType)
                {
                return transactionType == TransactionType.Income ? Colors.Green : Colors.Red;
                }
            return Colors.Black; // Default color
            }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
            throw new NotImplementedException();
            }
        }
    }