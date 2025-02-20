

namespace MoneyMap.Services;

public class CategoryService

    {

    private static SQLiteConnection _database = DatabaseService.GetConnection();

    public CategoryService()
        {
        _database = DatabaseService.GetConnection();
        }

    public static void EnsureDefaultBudgetCategories()
        {
        // 🔹 Fetch existing categories from the database
        var existingCategories = _database.Table<Category>().ToList();

        if (existingCategories.Count == 0)
            {
            Debug.WriteLine("[DEBUG] No categories found in the database. Default categories should already be initialized.");
            }
        else
            {
            Debug.WriteLine("[DEBUG] Categories loaded from the database:");
            foreach (var cat in existingCategories)
                {
                Debug.WriteLine($"- {cat.Name}");
                }
            }
        }









    public async Task<List<Category>> GetAllCategoriesAsync(bool debug = false)
        {
        var dbCategories = await Task.Run(() => _database.Table<Category>().ToList());

        if (debug)
            {
            Debug.WriteLine("[DEBUG] Fetched Categories:");
            foreach (var cat in dbCategories)
                {
                Debug.WriteLine($"-- {cat.Id}: {cat.Name}");
                }
            }

        return dbCategories;
        }







    public static void AddCategory(string categoryName)
        {
        var existingCategory = _database.Table<Category>()
            .FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());

        if (existingCategory == null)
            {
            _database.Insert(new Category { Name = categoryName });
            }
        }

    public static void DeleteCategory(int categoryId)
        {
        var category = _database.Table<Category>().FirstOrDefault(c => c.Id == categoryId);
        if (category != null)
            {
            _database.Execute("UPDATE Transaction SET CustomCategoryId = NULL WHERE CustomCategoryId = ?", categoryId);
            _database.Delete(category);
            _database.Execute("DELETE FROM BudgetCategory WHERE CategoryId = ?", categoryId);
            }
        }

    public static string GetCategoryNameById(int categoryId)
        {
        if (categoryId == 0)
            return "Uncategorized"; // Handle missing category IDs explicitly

        var category = _database.Table<Category>().FirstOrDefault(c => c.Id == categoryId);
        return category != null ? category.Name : "Unknown Category";
        }


    }
