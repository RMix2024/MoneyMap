
namespace MoneyMap.PageModels;

public partial class MainPageModel : ObservableObject
    {
    private readonly TransactionService _transactionService;
    private readonly BudgetService _budgetService;
    private readonly CategoryService _categoryService;
    private readonly ModalErrorHandler _errorHandler;

    private ObservableCollection<Transaction> _transactions = new();
    public ObservableCollection<Transaction> Transactions
        {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
        }


    [ObservableProperty]
    private ObservableCollection<BudgetCategory> _budgetCategories;

    private decimal _totalBalance;
    public decimal TotalBalance
        {
        get => _totalBalance;
        set => SetProperty(ref _totalBalance, value);
        }


    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private string _today = DateTime.Now.ToString("dddd, MMM d");

    public MainPageModel(TransactionService transactionService, BudgetService budgetService,
        CategoryService categoryService, ModalErrorHandler errorHandler)
        {
        _transactionService = transactionService;
        _budgetService = budgetService;
        _categoryService = categoryService;
        _errorHandler = errorHandler;
        _transactions = new ObservableCollection<Transaction>();
        _budgetCategories = new ObservableCollection<BudgetCategory>();

        _ = LoadData(); // Explicitly ignore result but keep async behavior
        }

    private async Task LoadData()
        {
        try
            {
            IsBusy = true;

            // Load transactions and budget categories
            var transactions = await _transactionService.GetTransactionsAsync();
            var budgetCategories = _budgetService.GetBudgetCategories();

            Transactions = new ObservableCollection<Transaction>(transactions);
            BudgetCategories = new ObservableCollection<BudgetCategory>(budgetCategories);

            UpdateBalance();
            }
        catch (Exception e)
            {
            _errorHandler.HandleError(e);
            }
        finally
            {
            IsBusy = false;
            }
        }

    private void UpdateBalance()
        {
        TotalBalance = Transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
        }

    [RelayCommand]
    private async Task Refresh()
        {
        try
            {
            IsRefreshing = true;
            await LoadData();
            }
        catch (Exception e)
            {
            _errorHandler.HandleError(e);
            }
        finally
            {
            IsRefreshing = false;
            }
        }

    [RelayCommand]
    private async Task AddTransaction()
        {
        await Shell.Current.GoToAsync("addTransactionPage");
        }

    [RelayCommand]
    private async Task EditTransaction(Transaction transaction)
        {
        if (transaction == null) return;

        await Shell.Current.GoToAsync($"editTransactionPage?id={transaction.Id}");
        }

    [RelayCommand]
    private async Task DeleteTransaction(Transaction transaction)
        {
        if (transaction == null) return;

        bool confirm = await Shell.Current.DisplayAlert("Confirm Delete",
     $"Are you sure you want to delete '{transaction.Description}'?", "Yes", "No");


        if (confirm)
            {
            await _transactionService.DeleteTransactionAsync(transaction);
            Transactions.Remove(transaction);
            UpdateBalance();
            }
        }
    }
