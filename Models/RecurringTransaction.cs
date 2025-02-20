namespace MoneyMap.Models
    {
    public class RecurringTransaction
        {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public int CategoryId { get; set; } // Links to Category table

        public TransactionType Type { get; set; } // Income or Expense

        public DateTime StartDate { get; set; }

        public int FrequencyDays { get; set; } // e.g., 30 for monthly, 7 for weekly

        public bool IsActive { get; set; } = true;
        }
    }
