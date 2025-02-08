using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using MoneyMap2.Models;

using MoneyMap2.Data;

namespace MoneyMap2.Services
{
    internal class DatabaseService
    {
        private readonly SQLiteConnection _database;

        public DatabaseService()
        {
            _database = new SQLiteConnection(DatabaseConfig.GetDatabasePath());
            _database.CreateTable<Transaction>(); // Creates table if it doesn’t exist
        }

        public void AddTransaction(Transaction transaction)
        {
            _database.Insert(transaction);
        }

        public List<Transaction> GetTransactions()
        {
            return _database.Table<Transaction>().ToList();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            _database.Update(transaction);
        }

        public void DeleteTransaction(Transaction transaction)
        {
            _database.Delete(transaction);
        }



    }
}
