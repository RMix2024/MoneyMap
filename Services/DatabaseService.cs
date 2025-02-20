
namespace MoneyMap.Services
    {
    public class DatabaseService
        {
        private static SQLiteConnection _database;
        private static readonly object Locker = new();

        // 🔹 Static Constructor Ensures One-Time Initialization
        static DatabaseService()
            {
            InitializeDatabase();

            }

        // 🔹 Ensure the database is initialized safely
        private static void InitializeDatabase()
            {
            Debug.WriteLine("InitializeDatabase() method was called.");

            try
                {
                if (_database == null)
                    {
                    Debug.WriteLine("Initializing SQLite Connection...");

                    string dbPath = DatabaseConfig.GetDatabasePath();
                    Debug.WriteLine($"Database Path: {dbPath}");

                    _database = new SQLiteConnection(dbPath);

                    Debug.WriteLine(" Creating Tables...");
                    _database.CreateTable<Transaction>();
                    _database.CreateTable<Category>();
                    _database.CreateTable<BudgetCategory>();

                    Debug.WriteLine(" Database initialized successfully.");

                    // ✅ Insert predefined categories **only if table is empty**

                    Debug.WriteLine("Calling EnsureDefaultCategories.");
                    EnsureDefaultCategories();
                    }
                }
            catch (Exception ex)
                {
                Debug.WriteLine($" [ERROR] Database Initialization Failed: {ex.Message}");
                Debug.WriteLine($" Stack Trace: {ex.StackTrace}");
                throw;
                }
            }

        //  Inserts predefined categories **only if they don’t exist**
        public static void EnsureDefaultCategories()
            {
            Debug.WriteLine(" EnsureDefaultCategories() method was called.");
            var existingCategories = _database.Table<Category>().ToList();
            var existingCount = _database.Table<Category>().Count();

            Debug.WriteLine($"[DEBUG] Current categories count: {existingCategories.Count}");
            if (existingCount == 0)
                {
                Debug.WriteLine("[DEBUG] No categories found, inserting default categories...");

                var defaultCategories = new List<Category>
                {
                    new Category { Name = "Food & Dining", IsPredefined = true },
                    new Category { Name = "Bills & Utilities", IsPredefined = true },
                    new Category { Name = "Shopping", IsPredefined = true },
                    new Category { Name = "Entertainment", IsPredefined = true },
                    new Category { Name = "Transportation", IsPredefined = true },
                    new Category { Name = "Health & Fitness", IsPredefined = true },
                    new Category { Name = "Savings & Investments", IsPredefined = true },
                    new Category { Name = "Other", IsPredefined = true }
                };

                _database.InsertAll(defaultCategories); // Bulk insert
                Debug.WriteLine("[DEBUG] Inserted Default Categories");
                }
            //  Print categories from DB after insertion
            existingCategories = _database.Table<Category>().ToList();
            Debug.WriteLine("[DEBUG] Categories in DB after initialization:");
            foreach (var category in existingCategories)
                {
                Debug.WriteLine($" - {category.Id}: {category.Name}");
                }
            }

        //  Always ensure database is initialized before returning it
        public static SQLiteConnection GetConnection()
            {
            if (_database == null)
                {
                Debug.WriteLine(" DatabaseService was not initialized before use. Initializing now...");
                InitializeDatabase();

                }
            return _database;
            }
        }
    }
