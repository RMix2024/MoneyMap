
namespace MoneyMap.Pages;

public partial class TransactionsPage : ContentPage
    {
    public TransactionsPage()
        {
        InitializeComponent();
        // In a production app, you might use dependency injection.
        BindingContext = new TransactionsPageModel(new TransactionService(), new CategoryService());
        }

    // Handler for the Edit Mode toggle.
    private void OnEditModeToggled(object sender, ToggledEventArgs e)
        {
        if (BindingContext is TransactionsPageModel vm)
            {
            vm.IsEditMode = e.Value;
            }
        }

    // Handler for Edit button clicks.
    private void OnEditTransactionClicked(object sender, EventArgs e)
        {
        if (sender is Button btn && btn.CommandParameter is Transaction transaction)
            {
            if (BindingContext is TransactionsPageModel vm &&
                vm.EditTransactionCommand.CanExecute(transaction))
                {
                vm.EditTransactionCommand.Execute(transaction);
                }
            }
        }

    // Handler for Delete button clicks.
    private void OnDeleteTransactionClicked(object sender, EventArgs e)
        {
        if (sender is Button btn && btn.CommandParameter is Transaction transaction)
            {
            if (BindingContext is TransactionsPageModel vm &&
                vm.DeleteTransactionCommand.CanExecute(transaction))
                {
                vm.DeleteTransactionCommand.Execute(transaction);
                }
            }
        }

    }
