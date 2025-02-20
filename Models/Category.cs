using SQLite;

namespace MoneyMap.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Name { get; set; } = string.Empty;

        // Predefined categories will have an initial `true` value
        public bool IsPredefined { get; set; } = false;
    }
}
