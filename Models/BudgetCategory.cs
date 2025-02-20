using SQLite;
using SQLiteNetExtensions.Attributes;


namespace MoneyMap.Models
{
    public class BudgetCategory
    {
        [PrimaryKey, AutoIncrement]  // ✅ This makes it a unique identifier
        public int Id { get; set; }

        public decimal BudgetAmount { get; set; } // Stores the budget amount

        [ForeignKey(typeof(Category))] // ✅ Link to Category table
        public int CategoryId { get; set; }

    }
}
