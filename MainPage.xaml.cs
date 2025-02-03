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
    public ObservableCollection<Transaction> RecentTransactions { get; set; } = new();

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

        // Ensure RecentTransactions updates properly
        RecentTransactions = new ObservableCollection<Transaction>(
            Transactions.OrderByDescending(t => t.Date).Take(5)
        );

        Console.WriteLine($"Recent Transactions Count: {RecentTransactions.Count}"); // Debug Log

        OnPropertyChanged(nameof(TotalBalance));
        OnPropertyChanged(nameof(RecentTransactions)); // Force UI update
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
                    Color = SKColor.Parse("#4CAF50") // Green
                },
                new ChartEntry((float)expenses)
                {
                    Label = "Expenses",
                    ValueLabel = expenses.ToString("C"),
                    Color = SKColor.Parse("#F44336") // Red
                }
            },
            BackgroundColor = SKColors.Transparent, // Make sure this is set
            HoleRadius = 0.5f
        };



        var categoryGroups = Transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => t.Category)
            .Select(g => new ChartEntry((float)g.Sum(t => t.Amount))
            {
                Label = g.Key.ToString(),
                ValueLabel = g.Sum(t => t.Amount).ToString("C"),
                Color = SKColor.Parse("#" + new Random().Next(0x1000000).ToString("X6")) // Random color
            }).ToArray();

        ExpenseBreakdownChart = new PieChart { Entries = categoryGroups, BackgroundColor = SKColors.Transparent
        };

        OnPropertyChanged(nameof(IncomeVsExpenseChart));
        OnPropertyChanged(nameof(ExpenseBreakdownChart));
    }
}
