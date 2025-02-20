
namespace MoneyMap.Converters
    {
    public class CategoryIdToNameConverter : IValueConverter
        {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            if (value is int categoryId)
                {
                var database = DatabaseService.GetConnection(); // Direct DB access
                var category = database.Table<Category>().FirstOrDefault(c => c.Id == categoryId);
                Debug.WriteLine($"Converting CategoryId {categoryId} → {(category?.Name ?? "Unknown Category")}");
                return category?.Name ?? "Unknown"; // Default if not found
                }
            return "Unknown";
            }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
            throw new NotImplementedException();
            }
        }
    }
