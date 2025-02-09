using MoneyMap2.Models;
using MoneyMap2.Services;
using MoneyMap2.Data;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System;
using CommunityToolkit.Maui.Alerts;


namespace MoneyMap2.Views;

public partial class TransactionsPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    public ObservableCollection<Transaction> Transactions { get; set; } = [];
    public List<string> TransactionTypes { get; } = new List<string> { "Select One", "Income", "Expense" };

    public List<string> Categories { get; } = new List<string>
    {
        "Select One", // Default unselected value
        "Food & Dining",
        "Bills & Utilities",
        "Shopping",
        "Entertainment",
        "Transportation",
        "Health & Fitness",
        "Savings & Investments",
        "Other"
    };

    private TransactionType? selectedTransactionType = null;
    private TransactionCategory? selectedCategory = null;
    private bool isEditMode = false;

    public TransactionsPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        LoadTransactions();
        BindingContext = this;

        TransactionTypePicker.SelectedIndex = 0;
        CategoryPicker.SelectedIndex = 0;
    }

    private void OnEditModeToggled(object sender, ToggledEventArgs e)
    {
        isEditMode = e.Value;
        TransactionsList.ItemsSource = null;
        TransactionsList.ItemsSource = Transactions; // Refresh UI
    }

    private void LoadTransactions()
    {
        Transactions.Clear();
        foreach (var transaction in _databaseService.GetTransactions())
        {
            Transactions.Add(transaction);
        }
    }

    private void OnTransactionTypeChanged(object sender, EventArgs e)
    {
        if (TransactionTypePicker.SelectedIndex > 0) // Ignore "Select One"
        {
            var selectedValue = TransactionTypePicker.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedValue))
            {
                selectedTransactionType = Enum.Parse<TransactionType>(selectedValue);

                // Hide category picker if "Income" is selected
                CategoryPicker.IsVisible = selectedTransactionType == TransactionType.Expense;
            }
            else
            {
                selectedTransactionType = null; // Prevents parsing null
            }
        }
        else
        {
            selectedTransactionType = null; // Reset if "Select One"
        }
    }

    public Dictionary<string, TransactionCategory> CategoryMapping { get; } = new Dictionary<string, TransactionCategory>
    {
        { "Food & Dining", TransactionCategory.Food_And_Dining },
        { "Bills & Utilities", TransactionCategory.Bills_And_Utilities },
        { "Shopping", TransactionCategory.Shopping },
        { "Entertainment", TransactionCategory.Entertainment },
        { "Transportation", TransactionCategory.Transportation },
        { "Health & Fitness", TransactionCategory.Health_And_Fitness },
        { "Savings & Investments", TransactionCategory.Savings_And_Investments },
        { "Other", TransactionCategory.Other }
    };

    private void OnCategoryChanged(object sender, EventArgs e)
    {
        if (CategoryPicker.SelectedIndex > 0) // Ignore "Select Category"
        {
            var selectedValue = CategoryPicker.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedValue) && CategoryMapping.ContainsKey(selectedValue))
            {
                selectedCategory = CategoryMapping[selectedValue];  // Map UI string to Enum
            }
            else
            {
                selectedCategory = null; // Prevents invalid selection
            }
        }
        else
        {
            selectedCategory = null; // Reset if "Select Category"
        }
    }

    private async void OnAddTransactionClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DescriptionEntry.Text))
        {
           await DisplayAlert("Error", "Please enter a description.", "OK");
            return;
        }

        if (!decimal.TryParse(AmountEntry.Text, out decimal amount))
        {
           await DisplayAlert("Error", "Invalid amount. Please enter a valid number.", "OK");
            return;
        }

        if (selectedTransactionType == null)
        {
            await DisplayAlert("Error", "Please select Income or Expense", "OK");
            return;
        }

        // Category is required **only for Expense transactions**
        if (selectedTransactionType == TransactionType.Expense && selectedCategory == null)
        {
           await DisplayAlert("Error", "Please select a category for expenses.", "OK");
            return;
        }

        var newTransaction = new Transaction
        {
            Description = DescriptionEntry.Text,
            Amount = amount,
            Date = DateTime.Now,
            Type = selectedTransactionType.Value,
            Category = selectedTransactionType == TransactionType.Expense ? selectedCategory.Value : null
        };

        _databaseService.AddTransaction(newTransaction);
        Transactions.Add(newTransaction);
        UpdateBalance();
        DescriptionEntry.Text = string.Empty;
        AmountEntry.Text = string.Empty;

        //Show Snackbar with Undo Option
        var snackbar = Snackbar.Make(
            "Transaction added successfully!",
            async () =>
            {
                // Undo: Remove last transaction
                Transactions.Remove(newTransaction);
                _databaseService.DeleteTransaction(newTransaction);
                UpdateBalance();
            },
            "Undo",
            TimeSpan.FromSeconds(3) // Show for 3 seconds
        );

        await snackbar.Show();
    }

    private async void OnEditTransactionClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var transaction = (Transaction)button.CommandParameter;

        string newDescription = await DisplayPromptAsync("Edit Transaction", "Enter new description:", initialValue: transaction.Description);
        string newAmountString = await DisplayPromptAsync("Edit Transaction", "Enter new amount:", keyboard: Keyboard.Numeric, initialValue: transaction.Amount.ToString());

        if (string.IsNullOrWhiteSpace(newDescription))
        {
            await DisplayAlert("Error", "Description cannot be empty.", "OK");
            return;
        }

        if (!decimal.TryParse(newAmountString, out decimal newAmount))
        {
            await DisplayAlert("Error", "Invalid amount entered.", "OK");
            return;
        }

        // Ask for transaction type change
        string action = await DisplayActionSheet("Select Transaction Type", "Cancel", null, "Income", "Expense");
        if (action == "Cancel") return;

        TransactionType newType = action == "Income" ? TransactionType.Income : TransactionType.Expense;

        // Only ask for category if it is an expense
        TransactionCategory? newCategory = transaction.Category; // Keep existing category by default
        if (newType == TransactionType.Expense)
        {
            string categoryAction = await DisplayActionSheet("Select Category", "Cancel", null, Categories.ToArray());
            if (categoryAction == "Cancel") return;

            if (CategoryMapping.ContainsKey(categoryAction))
            {
                newCategory = CategoryMapping[categoryAction];  // Update category
            }
            else
            {
                newCategory = null; // Prevent invalid selection
            }
        }
        else
        {
            newCategory = null; // If income, remove category
        }

        // Update Transaction
        transaction.Description = newDescription;
        transaction.Amount = newAmount;
        transaction.Type = newType;
        transaction.Category = newCategory; // Allow null for income transactions

        _databaseService.UpdateTransaction(transaction);
        TransactionsList.ItemsSource = null;
        TransactionsList.ItemsSource = Transactions; // Refresh UI
        UpdateBalance();
    }

    private async void OnDeleteTransactionClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var transaction = (Transaction)button.CommandParameter;

        bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete '{transaction.Description}'?", "Yes", "No");
        if (confirm)
        {
            Transactions.Remove(transaction);
            _databaseService.DeleteTransaction(transaction);
            UpdateBalance();

            TransactionsList.ItemsSource = null;
            TransactionsList.ItemsSource = Transactions; // Refresh UI
        }
    }

    public decimal TotalBalance
    {
        get => Transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
    }

    // Refresh the balance whenever transactions change
    private void UpdateBalance()
    {
        OnPropertyChanged(nameof(TotalBalance));
    }
}
