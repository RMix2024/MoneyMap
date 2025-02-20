namespace MoneyMap.PageModels
    {
    public partial class BudgetPageModel : ObservableObject
        {
        private readonly BudgetService _budgetService;
        private readonly ModalErrorHandler _errorHandler;

        [ObservableProperty]
        private ObservableCollection<BudgetCategory> _budgetCategories = new();

        [ObservableProperty]
        private decimal _totalBudget;

        public BudgetPageModel(BudgetService budgetService, ModalErrorHandler errorHandler)
            {
            _budgetService = budgetService;
            _errorHandler = errorHandler;
            BudgetCategories = new ObservableCollection<BudgetCategory>(); // Ensure proper initialization

            Task.Run(async () => await LoadBudgetData()).Wait(); // Seed when page loads

            // Track budget changes and auto-save
            BudgetCategories.CollectionChanged += (s, e) =>
            {
                foreach (var item in e.NewItems?.OfType<BudgetCategory>() ?? Enumerable.Empty<BudgetCategory>())
                    {
                    item.PropertyChanged += (s2, e2) =>
                    {
                        if (e2.PropertyName == nameof(item.BudgetAmount))
                            {
                            _budgetService.AddOrUpdateBudget(item);
                            }
                    };
                    }
            };
            }

        private async Task LoadBudgetData()
            {
            try
                {
                _budgetService.SeedBudgetCategories(); // Ensure budget categories exist
                var budgetCategories = _budgetService.GetBudgetCategories(); // Fix: Await Async Method
                BudgetCategories = new ObservableCollection<BudgetCategory>(budgetCategories);
                OnPropertyChanged(nameof(BudgetCategories));  // Notify UI about changes
                UpdateTotalBudget();
                }
            catch (Exception e)
                {
                _errorHandler.HandleError(e);
                }
            }

        private void UpdateTotalBudget()
            {
            if (BudgetCategories != null && BudgetCategories.Any())
                {
                TotalBudget = BudgetCategories.Sum(c => c.BudgetAmount);
                }
            else
                {
                TotalBudget = 0;
                }
            }

        [RelayCommand]
        private async Task AddBudgetCategory()
            {
            await Shell.Current.GoToAsync("addBudgetCategoryPage");
            }
        }
    }
