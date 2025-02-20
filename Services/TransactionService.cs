namespace MoneyMap.Services
    {
    public class TransactionService
        {
        private readonly SQLiteConnection _database;

        public TransactionService()
            {
            _database = DatabaseService.GetConnection();
            }

        public async Task<List<Transaction>> GetTransactionsAsync()
            {
            return await Task.Run(() => _database.Table<Transaction>().ToList());
            }

        public async Task AddTransactionAsync(Transaction transaction)
            {
            await Task.Run(() => _database.Insert(transaction));
            }

        public async Task UpdateTransactionAsync(Transaction transaction)
            {
            await Task.Run(() => _database.Update(transaction));
            }

        public async Task DeleteTransactionAsync(Transaction transaction)
            {
            await Task.Run(() => _database.Delete(transaction));
            }
        }
    }
