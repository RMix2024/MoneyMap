namespace MoneyMap.Models
    {
    public class BudgetCategory : INotifyPropertyChanged
        {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private decimal _budgetAmount; // ✅ Backing field

        public decimal BudgetAmount
            {
            get => _budgetAmount;
            set
                {
                if (_budgetAmount != value)
                    {
                    _budgetAmount = value;
                    OnPropertyChanged(nameof(BudgetAmount)); // ✅ Notify UI of change
                    }
                }
            }

        [ForeignKey(typeof(Category))]
        public int CategoryId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) // ✅ Fix: Ensure Method Exists
            {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
