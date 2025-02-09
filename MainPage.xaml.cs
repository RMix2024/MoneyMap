using MoneyMap2.Models;
using MoneyMap2.Services;
using LiveChartsCore.SkiaSharpView.Maui;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Drawing;


namespace MoneyMap2;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    public ObservableCollection<Transaction> Transactions { get; set; } = new();
    public ObservableCollection<Transaction> RecentTransactions { get; set; } = new();

    public ISeries[] IncomeVsExpenseSeries { get; set; }
    public ISeries[] ExpenseBreakdownSeries { get; set; }

    public decimal TotalBalance => Transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);

    public MainPage()
    {
        InitializeComponent();
        Title = "MoneyMap";

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

        // ✅ Check if transactions exist
        if (Transactions.Count == 0 || (income == 0 && expenses == 0))
        {
            // ✅ Completely clear the charts to hide them
            IncomeVsExpenseSeries = Array.Empty<ISeries>();
            ExpenseBreakdownSeries = Array.Empty<ISeries>();

            OnPropertyChanged(nameof(IncomeVsExpenseSeries));
            OnPropertyChanged(nameof(ExpenseBreakdownSeries));
            return; // ✅ Exit early to prevent unnecessary processing
        }

        IncomeVsExpenseSeries = new ISeries[]
        {
        new PieSeries<double>
        {
            Values = new double[] { (double)income },
            Name = "Income",
            Fill = new SolidColorPaint(SKColors.Green),
            DataLabelsPaint = new SolidColorPaint(SKColors.White),
            DataLabelsSize = 18,
            DataLabelsPosition = PolarLabelsPosition.Middle,
            DataLabelsFormatter = point => $"Income: {point.Model:N2}",
            DataLabelsPadding = new Padding(10),
             Pushout= 5

        },
        new PieSeries<double>
        {
            Values = new double[] { (double)expenses },
            Name = "Expenses",
            Fill = new SolidColorPaint(SKColors.Red),
            DataLabelsPaint = new SolidColorPaint(SKColors.White),
            DataLabelsSize = 18,
            DataLabelsPosition = PolarLabelsPosition.Middle,
            DataLabelsFormatter = point => $"Expense: {point.Model:N2}",
             DataLabelsPadding = new Padding(10),
              Pushout= 5
        }
        };

        var categoryGroups = Transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => t.Category)
            .Select(g => new PieSeries<double>
            {
                Values = new double[] { (double)g.Sum(t => t.Amount) },
                Name = g.Key.ToString(),
                Fill = new SolidColorPaint(SKColor.Parse("#" + new Random().Next(0x1000000).ToString("X6"))),
                DataLabelsPaint = new SolidColorPaint(SKColors.White),
                DataLabelsSize = 18,
                DataLabelsPosition = PolarLabelsPosition.Middle,
                DataLabelsFormatter = point => $"{point.Context.Series.Name}:{point.Model:N2} ", // ✅ Display category + amount
                DataLabelsPadding = new Padding(10),
                Pushout= 5
            })
            .ToArray();

        ExpenseBreakdownSeries = categoryGroups;

        OnPropertyChanged(nameof(IncomeVsExpenseSeries));
        OnPropertyChanged(nameof(ExpenseBreakdownSeries));
    }


}
