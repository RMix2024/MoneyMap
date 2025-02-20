

namespace MoneyMap.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
        private async void OnTransactionsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//transactions");
        }

        private async void OnBudgetsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//budget");
        }

        private async void OnReportsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//reports");
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//settings");
        }
    }
}