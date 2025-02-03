

namespace MoneyMap
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


         // Apply Windows-specific style globally
#if WINDOWS
            Resources["DefaultPageStyle"] = Resources["WindowsPageStyle"];
#endif
        
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}