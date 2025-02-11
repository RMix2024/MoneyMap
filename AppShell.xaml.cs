﻿
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls; // For .NET MAUI Shell Navigation
using MoneyMap2; // Add this if MainPage is in the main namespace
using Microsoft.Maui.Controls; // For .NET MAUI Shell Navigation
using System.Collections.ObjectModel;


namespace MoneyMap2;




public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

    }



    private async void GoToTransactions(object sender, EventArgs e) => await Shell.Current.GoToAsync("//TransactionsPage");



    private async void GoToBudget(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//BudgetPage");
    }



    private async void GoToReports(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ReportsPage");
    }



    private async void GoToSettings(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SettingsPage");
    }



    private async void GoToTransActionForm(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//TransActionForm");
    }



    private async void GoToLoginPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }


}
