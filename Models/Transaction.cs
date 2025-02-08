using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MoneyMap2.Models
{
    public enum TransactionType
    {
        Income,
        Expense
    }

    public enum TransactionCategory
    {
        Food_And_Dining,
        Bills_And_Utilities,
        Shopping,
        Entertainment,
        Transportation,
        Health_And_Fitness,
        Savings_And_Investments,
        Other
    }
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public TransactionCategory? Category { get; set; }

        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
    }
}
