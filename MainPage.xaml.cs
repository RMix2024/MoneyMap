//using MoneyMap.Models;
//using MoneyMap.Services;
//using Microcharts;
//using SkiaSharp;
//using System.Collections.ObjectModel;
//using System.Linq;

//namespace MoneyMap;

//public partial class MainPage : ContentPage
//{
//    private readonly DatabaseService _databaseService;
//    public ObservableCollection<Transaction> Transactions { get; set; } = new();

//    private ObservableCollection<Transaction> _recentTransactions = new();

//    public ObservableCollection<Transaction> RecentTransactions
//    {
//        get => _recentTransactions;
//        set
//        {
//            _recentTransactions = value;
//            OnPropertyChanged(nameof(RecentTransactions)); // Notify UI
//        }
//    }

//    public Chart IncomeVsExpenseChart { get; set; }
//    public Chart ExpenseBreakdownChart { get; set; }

//    public decimal TotalBalance => Transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);



//    public MainPage()
//    {
//        InitializeComponent();
//        BindingContext = this;
//        _databaseService = new DatabaseService();
//        LoadTransactions();
//        GenerateCharts();
//    }

//    private void LoadTransactions()
//    {
//        Transactions.Clear();
//        foreach (var transaction in _databaseService.GetTransactions())
//        {
//            Transactions.Add(transaction);
//        }

//        Console.WriteLine($"Total Transactions: {Transactions.Count}"); // Debug Log

//        // Fix: Ensure RecentTransactions UI updates properly
//        RecentTransactions.Clear(); // Don't replace, just clear
//        foreach (var transaction in Transactions.OrderByDescending(t => t.Date).Take(5))
//        {
//            RecentTransactions.Add(transaction);
//        }

//        Console.WriteLine($"Final Recent Transactions Count: {RecentTransactions.Count}");

//        // 🔥 UI Refresh Fix: Ensure update is forced on the main thread
//        MainThread.BeginInvokeOnMainThread(() =>
//        {
//            OnPropertyChanged(nameof(TotalBalance));
//            OnPropertyChanged(nameof(RecentTransactions));
//        });
//    }





//    private void GenerateCharts()
//    {
//        var income = Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
//        var expenses = Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);


//        IncomeVsExpenseChart = new DonutChart
//        {
//            Entries = new[]
//            {
//                new ChartEntry((float)income)
//                {
//                    Label = "Income",
//                    ValueLabel = income.ToString("C"),
//                    Color = SKColor.Parse("#4CAF50") // Green
//                },
//                new ChartEntry((float)expenses)
//                {
//                    Label = "Expenses",
//                    ValueLabel = expenses.ToString("C"),
//                    Color = SKColor.Parse("#F44336") // Red
//                }
//            },
//            BackgroundColor = SKColors.Transparent, // Make sure this is set
//            HoleRadius = 0.5f
//        };



//        var categoryGroups = Transactions
//            .Where(t => t.Type == TransactionType.Expense)
//            .GroupBy(t => t.Category)
//            .Select(g => new ChartEntry((float)g.Sum(t => t.Amount))
//            {
//                Label = g.Key.ToString(),
//                ValueLabel = g.Sum(t => t.Amount).ToString("C"),
//                Color = SKColor.Parse("#" + new Random().Next(0x1000000).ToString("X6")) // Random color
//            }).ToArray();

//        ExpenseBreakdownChart = new PieChart
//        {
//            Entries = categoryGroups,
//            BackgroundColor = SKColors.Transparent
//        };

//        OnPropertyChanged(nameof(IncomeVsExpenseChart));
//        OnPropertyChanged(nameof(ExpenseBreakdownChart));
//        MainThread.BeginInvokeOnMainThread(() =>
//        {
//            RecentTransactionsList.ItemsSource = null;  // Force Unbind
//            RecentTransactionsList.ItemsSource = RecentTransactions;  // Rebind Data
//        });

//    }
//}


using MoneyMap.Models;
using MoneyMap.Services;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyMap;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    public ObservableCollection<Transaction> Transactions { get; set; } = new();

    private ObservableCollection<Transaction> _recentTransactions = new();

    public ObservableCollection<Transaction> RecentTransactions
    {
        get => _recentTransactions;
        set
        {
            _recentTransactions = value;
            OnPropertyChanged(nameof(RecentTransactions)); // Notify UI
        }
    }

    public Chart IncomeVsExpenseChart { get; set; }
    public Chart ExpenseBreakdownChart { get; set; }

    public decimal TotalBalance => Transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        _databaseService = new DatabaseService();
        LoadTransactions();
        GenerateCharts();
    }

    private void LoadTransactions()
    {
        Transactions.Clear();
        foreach (var transaction in _databaseService.GetTransactions())
        {
            Transactions.Add(transaction);
        }

        Console.WriteLine($"Total Transactions: {Transactions.Count}"); // Debug Log

        // ✅ Properly update RecentTransactions instead of replacing it
        MainThread.BeginInvokeOnMainThread(() =>
        {
            RecentTransactions.Clear(); // ✅ Clears the list without reassigning
            foreach (var transaction in Transactions.OrderByDescending(t => t.Date).Take(5))
            {
                RecentTransactions.Add(transaction);
            }

            Console.WriteLine($"Final Recent Transactions Count: {RecentTransactions.Count}");

            // ✅ Notify UI
            OnPropertyChanged(nameof(TotalBalance));
            OnPropertyChanged(nameof(RecentTransactions));
        });
    }


    private void GenerateCharts()
    {
        var income = Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var expenses = Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

        IncomeVsExpenseChart = new DonutChart
        {
            Entries = new[]
            {
                new ChartEntry((float)income)
                {
                    Label = "Income",
                    ValueLabel = income.ToString("C"),
                    Color = SKColor.Parse("#4CAF50"), // Green
                    TextColor = SKColors.White, // 🛠️ Change amount label color
                     ValueLabelColor = SKColor.Parse("#FFFFFF"), // ✅ Fixes Amount Text Color
                },
                new ChartEntry((float)expenses)
                {
                    Label = "Expenses",
                    ValueLabel = expenses.ToString("C"),
                    Color = SKColor.Parse("#F44336"), // Red
                    TextColor = SKColors.White, // 🛠️ Change amount label color
                     ValueLabelColor = SKColor.Parse("#FFFFFF"), // ✅ Fixes Amount Text Color
                }
            },
            BackgroundColor = SKColors.Transparent,
            HoleRadius = 0.5f,
            LabelTextSize = 40 // ✅ Increase label size
        };

        var categoryGroups = Transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => t.Category)
            .Select(g => new ChartEntry((float)g.Sum(t => t.Amount))
            {
                Label = g.Key.ToString(),
                ValueLabel = g.Sum(t => t.Amount).ToString("C"),
                Color = SKColor.Parse("#" + new Random().Next(0x1000000).ToString("X6")), // Random color
                 ValueLabelColor = SKColor.Parse("#FFFFFF"), // ✅ Fixes Amount Text Color
            }).ToArray();

        ExpenseBreakdownChart = new PieChart
        {
            Entries = categoryGroups,
            BackgroundColor = SKColors.Transparent,
            LabelTextSize = 40
            
        };

        // ✅ Notify UI updates
        OnPropertyChanged(nameof(IncomeVsExpenseChart));
        OnPropertyChanged(nameof(ExpenseBreakdownChart));
    }
}

