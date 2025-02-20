namespace MoneyMap.PageModels
    {
    // ObservableObject should implement INotifyPropertyChanged.
    public class TransactionsPageModel : ObservableObject
        {
        private readonly TransactionService _transactionService;
        private readonly CategoryService _categoryService;

        public ObservableCollection<Transaction> Transactions { get; set; } = new();
        public ObservableCollection<string> Categories { get; set; } = new();
        public List<string> TransactionTypes { get; } = new List<string> { "Select One", "Income", "Expense" };

        // Mapping of category names to IDs.
        private Dictionary<string, int> CategoryMapping = new Dictionary<string, int>();

        private decimal _totalBalance;
        public decimal TotalBalance
            {
            get => _totalBalance;
            private set => SetProperty(ref _totalBalance, value);
            }

        private DateTime _selectedTransactionDate = DateTime.Today;
        public DateTime SelectedTransactionDate
            {
            get => _selectedTransactionDate;
            set => SetProperty(ref _selectedTransactionDate, value);
            }

        // Property to control Edit Mode.
        private bool _isEditMode;
        public bool IsEditMode
            {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
            }

        public ICommand LoadTransactionsCommand { get; }
        public ICommand AddTransactionCommand { get; }
        public ICommand EditTransactionCommand { get; }
        public ICommand DeleteTransactionCommand { get; }

        public TransactionsPageModel(TransactionService transactionService, CategoryService categoryService)
            {
            _transactionService = transactionService;
            _categoryService = categoryService;

            LoadTransactionsCommand = new Command(async () => await LoadTransactions());
            AddTransactionCommand = new Command(async () => await OnAddTransaction());
            DeleteTransactionCommand = new Command<Transaction>(async (transaction) => await OnDeleteTransaction(transaction));
            EditTransactionCommand = new Command<Transaction>(async (transaction) => await OnEditTransaction(transaction));

            // Load categories and transactions at startup.
            LoadCategories();
            LoadTransactions();
            }

        private async Task LoadTransactions()
            {
            Transactions.Clear();
            var transactions = await _transactionService.GetTransactionsAsync();

            foreach (var transaction in transactions)
                {
                Transactions.Add(transaction);
                }

            Debug.WriteLine($"Loaded {Transactions.Count} transactions.");
            UpdateBalance();
            }

        private async void LoadCategories()
            {
            var categoryList = await _categoryService.GetAllCategoriesAsync();

            Categories.Clear();
            // Add a default placeholder.
            Categories.Add("Select Category");
            CategoryMapping.Clear();

            foreach (var category in categoryList)
                {
                Categories.Add(category.Name);
                CategoryMapping[category.Name] = category.Id;
                }

            OnPropertyChanged(nameof(Categories));
            }

        private void UpdateBalance()
            {
            TotalBalance = Transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
            Debug.WriteLine($"New balance: {TotalBalance}");
            }

        private async Task OnAddTransaction()
            {
            // 1) Get the main Window and its Page
            var window = Application.Current?.Windows?.FirstOrDefault();
            if (window?.Page is null) return;

            // 2) Use that Page to show prompts
            string description = await window.Page.DisplayPromptAsync("New Transaction", "Enter description:");
            if (string.IsNullOrWhiteSpace(description))
                return;

            string amountInput = await window.Page.DisplayPromptAsync("New Transaction", "Enter amount:", keyboard: Keyboard.Numeric);
            if (!decimal.TryParse(amountInput, out decimal amount))
                {
                await window.Page.DisplayAlert("Error", "Invalid amount.", "OK");
                return;
                }

            string typeSelection = await window.Page.DisplayActionSheet("Select Transaction Type", "Cancel", null, "Income", "Expense");
            if (typeSelection == "Cancel")
                return;

            TransactionType type = typeSelection == "Income" ? TransactionType.Income : TransactionType.Expense;
            int? categoryId = null;

            if (type == TransactionType.Expense)
                {
                string categorySelection = await window.Page.DisplayActionSheet("Select Category", "Cancel", null, Categories.ToArray());
                if (categorySelection == "Cancel")
                    return;

                if (CategoryMapping.ContainsKey(categorySelection))
                    {
                    categoryId = CategoryMapping[categorySelection];
                    }
                }

            DateTime selectedDate = await PickDateAsync(); // see note below

            var newTransaction = new Transaction
                {
                Description = description,
                Amount = amount,
                Date = selectedDate,
                Type = type,
                CategoryId = categoryId ?? 0
                };

            await _transactionService.AddTransactionAsync(newTransaction);
            Transactions.Add(newTransaction);
            UpdateBalance();
            }


        private async Task OnEditTransaction(Transaction transaction)
            {
            // Prompt for new description
            string newDescription = await Shell.Current.DisplayPromptAsync(
                "Edit Transaction",
                "Enter new description:",
                initialValue: transaction.Description
            );
            if (string.IsNullOrWhiteSpace(newDescription))
                {
                await Shell.Current.DisplayAlert("Error", "Description cannot be empty.", "OK");
                return;
                }

            // Prompt for new amount
            string newAmountString = await Shell.Current.DisplayPromptAsync(
                "Edit Transaction",
                "Enter new amount:",
                keyboard: Keyboard.Numeric,
                initialValue: transaction.Amount.ToString()
            );
            if (!decimal.TryParse(newAmountString, out decimal newAmount))
                {
                await Shell.Current.DisplayAlert("Error", "Invalid amount entered.", "OK");
                return;
                }

            // Prompt for transaction type
            string action = await Shell.Current.DisplayActionSheet(
                "Select Transaction Type",
                "Cancel",
                null,
                "Income",
                "Expense"
            );
            if (action == "Cancel")
                return;

            TransactionType newType = action == "Income" ? TransactionType.Income : TransactionType.Expense;
            int? newCategory = transaction.CategoryId;

            // If expense, prompt for category
            if (newType == TransactionType.Expense)
                {
                string categoryAction = await Shell.Current.DisplayActionSheet(
                    "Select Category",
                    "Cancel",
                    null,
                    Categories.ToArray()
                );
                if (categoryAction == "Cancel")
                    return;

                if (CategoryMapping.ContainsKey(categoryAction))
                    {
                    newCategory = CategoryMapping[categoryAction];
                    }
                else
                    {
                    newCategory = null;
                    }
                }
            else
                {
                newCategory = null;
                }

            // Update transaction and save
            transaction.Description = newDescription;
            transaction.Amount = newAmount;
            transaction.Type = newType;
            transaction.CategoryId = newCategory ?? 0;

            await _transactionService.UpdateTransactionAsync(transaction);
            UpdateBalance();
            }


        private async Task OnDeleteTransaction(Transaction transaction)
            {
            bool confirm = await Shell.Current.DisplayAlert("Confirm Delete", $"Delete '{transaction.Description}'?", "Yes", "No");
            if (!confirm)
                return;

            Transactions.Remove(transaction);
            await _transactionService.DeleteTransactionAsync(transaction);
            UpdateBalance();
            }

        // Helper method to show a date picker popup.
        private async Task<DateTime> PickDateAsync()
            {
            var tcs = new TaskCompletionSource<DateTime>();

            var datePicker = new DatePicker
                {
                Date = DateTime.Today,
                HorizontalOptions = LayoutOptions.Center
                };

            var acceptButton = new Button
                {
                Text = "OK",
                WidthRequest = 100,
                HorizontalOptions = LayoutOptions.Center
                };

            var cancelButton = new Button
                {
                Text = "Cancel",
                WidthRequest = 100,
                HorizontalOptions = LayoutOptions.Center
                };

            acceptButton.Clicked += (s, e) => tcs.TrySetResult(datePicker.Date);
            cancelButton.Clicked += (s, e) => tcs.TrySetResult(DateTime.Today);

            var contentLayout = new VerticalStackLayout
                {
                Spacing = 15,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children = { datePicker, acceptButton, cancelButton }
                };

            var popup = new ContentPage
                {
                Content = new Border
                    {
                    Content = contentLayout,
                    BackgroundColor = Color.FromArgb("#252525"),
                    Padding = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                    }
                };

            await Shell.Current.Navigation.PushModalAsync(popup);
            var result = await tcs.Task;
            await Shell.Current.Navigation.PopModalAsync();

            return result;
            }
        }
    }
