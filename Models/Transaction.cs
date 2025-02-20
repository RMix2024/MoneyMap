namespace MoneyMap.Models
    {
    public enum TransactionType
        {
        Income,
        Expense
        }


    public class Transaction
        {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty; // Allows null for optional descriptions

        public decimal Amount { get; set; }

        public int CategoryId { get; set; } // Foreign key for categories

        public DateTime Date { get; set; }

        public TransactionType Type { get; set; }
        }
    }
