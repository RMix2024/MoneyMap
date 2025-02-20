namespace MoneyMap.Pages
    {
    public partial class BudgetPage : ContentPage
        {
        public BudgetPage(BudgetPageModel model)
            {
            InitializeComponent();
            BindingContext = model;
            }
        }
    }
