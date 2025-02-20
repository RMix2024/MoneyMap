


namespace MoneyMap
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit(options =>
                {
                    options.SetShouldEnableSnackbarOnWindows(true);
                })
                
                .ConfigureMauiHandlers(handlers =>
                {
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                    fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
                });

            builder.ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
events.AddWindows(w => w.OnWindowCreated(window =>
{
    var nativeWindow = (Microsoft.UI.Xaml.Window)window;
    nativeWindow.Activate();

    //  Get the WindowId
    var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
    var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);

    //  Get the AppWindow instance
    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

    if (appWindow != null)
    {
        appWindow.Resize(new Windows.Graphics.SizeInt32(1200, 800)); // Set Width x Height
    }
}));
#endif

            });


#if DEBUG
            builder.Logging.AddDebug();
    		builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            builder.Services.AddSingleton<BudgetService>();
            builder.Services.AddSingleton<CategoryService>();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<TransactionService>();
            builder.Services.AddSingleton<ModalErrorHandler>();
            builder.Services.AddSingleton<MainPageModel>();


            //builder.Services.AddTransientWithShellRoute<ProjectDetailPage, ProjectDetailPageModel>("project");
            //builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task");
           
            return builder.Build();
        }
    }
}
