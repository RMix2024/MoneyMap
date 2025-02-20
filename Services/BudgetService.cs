namespace MoneyMap.Services
    {
    public class BudgetService
        {
        private readonly SQLiteConnection _database;

        public BudgetService()
            {
            _database = DatabaseService.GetConnection(); // Match other services
            }

        public List<BudgetCategory> GetBudgetCategories()
            {
            return _database.Table<BudgetCategory>().ToList();
            }

        public void SeedBudgetCategories()
            {
            var existingBudgets = _database.Table<BudgetCategory>().ToList();
            var existingCategories = _database.Table<Category>().ToList();

            Debug.WriteLine($"Existing Budgets: {existingBudgets.Count}");
            Debug.WriteLine($"Existing Categories: {existingCategories.Count}");

            // Remove orphaned budget categories (ones that reference deleted categories)
            var orphanedBudgets = existingBudgets.Where(b => !existingCategories.Any(c => c.Id == b.CategoryId)).ToList();
            foreach (var orphan in orphanedBudgets)
                {
                Debug.WriteLine($"Removing orphaned budget: CategoryId {orphan.CategoryId}");
                _database.Delete(orphan);
                }

            // Add missing budget categories for existing categories
            foreach (var category in existingCategories)
                {
                if (!existingBudgets.Any(b => b.CategoryId == category.Id))
                    {
                    Debug.WriteLine($"Seeding budget category for CategoryId {category.Id}");
                    _database.Insert(new BudgetCategory
                        {
                        CategoryId = category.Id,
                        BudgetAmount = 100 // Default budget amount
                        });
                    }
                }
            }



        public void AddOrUpdateBudget(BudgetCategory budget)
            {
            var existingBudget = _database.Table<BudgetCategory>()
                .FirstOrDefault(b => b.CategoryId == budget.CategoryId);

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
