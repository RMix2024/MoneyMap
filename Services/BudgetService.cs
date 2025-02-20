using MoneyMap.Models;
using SQLite;
using MoneyMap.Services;

namespace MoneyMap.Services
{
   public class BudgetService
    {
        private readonly SQLiteConnection _database;

        public BudgetService()
        {
            _database = DatabaseService.GetConnection();
        }


        public List<BudgetCategory> GetBudgetCategories()
        {
            var storedBudgets = _database.Table<BudgetCategory>().ToList(); // Fetch from database
            var predefinedCategories = _database.Table<Category>().Where(c => c.IsPredefined).ToList(); // Get predefined categories

            // Ensure all predefined categories exist in storedBudgets
            foreach (var category in predefinedCategories)
            {
                if (!storedBudgets.Any(b => b.CategoryId == category.Id))
                {
                    storedBudgets.Add(new BudgetCategory
                    {
                        CategoryId = category.Id,
                        BudgetAmount = 0 // Default to zero
                    });
                }
            }

            return storedBudgets;
        }








        public void AddOrUpdateBudget(BudgetCategory budget)
        {
            var existingBudget = _database.Table<BudgetCategory>()
                .FirstOrDefault(b => b.CategoryId == budget.CategoryId); // Compare by CategoryId

            if (existingBudget != null)
            {
                existingBudget.BudgetAmount = budget.BudgetAmount;
                _database.Update(existingBudget);
            }
            else
            {
                _database.Insert(budget);
            }
        }


    }
}
