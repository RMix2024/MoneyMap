using MoneyMap.Models;
using MoneyMap.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System;

namespace MoneyMap;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    public ObservableCollection<Transaction> Transactions { get; set; } = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        _databaseService = new DatabaseService();
       

    }

   
}
