
using MoneyMap.Models;
using MoneyMap.Services;
using MoneyMap.Data;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System;

namespace MoneyMap.Views;


public partial class TransactionsPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    public ObservableCollection<Transaction> Transactions { get; set; } = [];
    public List<string> TransactionTypes { get; } = new List<string> { "Select One", "Income", "Expense" };
    private TransactionType? selectedTransactionType = null;
    private bool isEditMode = false;

    public TransactionsPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        LoadTransactions();
        BindingContext = this;
        TransactionTypePicker.SelectedIndex = 0;

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




    private void OnAddTransactionClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DescriptionEntry.Text))
        {
            DisplayAlert("Error", "Please enter a description.", "OK");
            return;
        }

        if (!decimal.TryParse(AmountEntry.Text, out decimal amount))
        {
            DisplayAlert("Error", "Invalid amount. Please enter a valid number.", "OK");
            return;
        }

        if (selectedTransactionType == null)
        {
            DisplayAlert("Error", "Please select Income or Expense", "OK");
            return;
        }

        var newTransaction = new Transaction
        {
            Description = DescriptionEntry.Text,
            Amount = amount,
            Date = DateTime.Now,
            Type = selectedTransactionType.Value
        };

        _databaseService.AddTransaction(newTransaction);
        Transactions.Add(newTransaction);
        UpdateBalance();
        DescriptionEntry.Text = string.Empty;
        AmountEntry.Text = string.Empty;
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
        if (action != "Income" && action != "Expense") return;

        // Update Transaction
        transaction.Description = newDescription;
        transaction.Amount = newAmount;
        transaction.Type = action == "Income" ? TransactionType.Income : TransactionType.Expense;

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

