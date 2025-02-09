using SkiaSharp.Views.Maui.Controls.Hosting;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microsoft.Maui.LifecycleEvents;



namespace MoneyMap2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
                .UseSkiaSharp() // ✅ Correct way to enable SkiaSharp
            .UseLiveCharts()
            .UseMauiCommunityToolkit(); // ✅ Ensure LiveCharts is properly initialized
                                        // ✅ Windows-Only Configuration
            builder.ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
events.AddWindows(w => w.OnWindowCreated(window =>
{
    var nativeWindow = (Microsoft.UI.Xaml.Window)window;
    nativeWindow.Activate();

    // ✅ Get the WindowId
    var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
    var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);

    // ✅ Get the AppWindow instance
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
#endif
            return builder.Build();
        }
    }
}