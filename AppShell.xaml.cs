namespace MoneyMap;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Get current theme and set SegmentedControl
        var currentTheme = Application.Current?.UserAppTheme ?? AppTheme.Light;
        ThemePicker.SelectedIndex = currentTheme == AppTheme.Light ? 0 : 1;
    }

    private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker.SelectedIndex == 0)
            Application.Current!.UserAppTheme = AppTheme.Light;
        else
            Application.Current!.UserAppTheme = AppTheme.Dark;
    }



    // Display Snackbar
    public static async Task DisplaySnackbarAsync(string message)
    {
        CancellationTokenSource cancellationTokenSource = new();

        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Color.FromArgb("#FF3300"),
            TextColor = Colors.White,
            ActionButtonTextColor = Colors.Yellow,
            CornerRadius = new CornerRadius(0),
            Font = Font.SystemFontOfSize(18),
            ActionButtonFont = Font.SystemFontOfSize(14)
        };

        var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);
        await snackbar.Show(cancellationTokenSource.Token);
    }

    // Display Toast
    public static async Task DisplayToastAsync(string message)
    {
        // Toast does not work on Windows in MCT
        if (OperatingSystem.IsWindows())
            return;

        try
        {
            var toast = Toast.Make(message, textSize: 18);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await toast.Show(cts.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Toast failed: {ex.Message}");
        }
    }

    //// Handle Theme Change in SegmentedControl
    //private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    //{
    //    if (Application.Current != null)
    //    {
    //        Application.Current.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
    //    }
    //}
}


//namespace MoneyMap;

//    public partial class AppShell : Shell
//    {
//        public AppShell()
//        {
//            InitializeComponent();
//            var currentTheme = Application.Current!.UserAppTheme;
//            ThemeSegmentedControl.SelectedIndex = currentTheme == AppTheme.Light ? 0 : 1;
//        }
//        public static async Task DisplaySnackbarAsync(string message)
//        {
//            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

//            var snackbarOptions = new SnackbarOptions
//            {
//                BackgroundColor = Color.FromArgb("#FF3300"),
//                TextColor = Colors.White,
//                ActionButtonTextColor = Colors.Yellow,
//                CornerRadius = new CornerRadius(0),
//                Font = Font.SystemFontOfSize(18),
//                ActionButtonFont = Font.SystemFontOfSize(14)
//            };

//            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);

//            await snackbar.Show(cancellationTokenSource.Token);
//        }

//        public static async Task DisplayToastAsync(string message)
//        {
//            // Toast is currently not working in MCT on Windows
//            if (OperatingSystem.IsWindows())
//                return;

//            var toast = Toast.Make(message, textSize: 18);

//            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
//            await toast.Show(cts.Token);
//        }

//        private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
//        {
//            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
//        }
//    }

