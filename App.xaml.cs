namespace MoneyMap
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Register routes centrally
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(TransactionsPage), typeof(TransactionsPage));
            Routing.RegisterRoute(nameof(BudgetPage), typeof(BudgetPage));
            Routing.RegisterRoute(nameof(ReportsPage), typeof(ReportsPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            //Routing.RegisterRoute(nameof(TransActionForm), typeof(TransActionForm));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}