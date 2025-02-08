using SkiaSharp.Views.Maui.Controls.Hosting;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

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
            }).UseSkiaSharp() // ✅ Correct way to enable SkiaSharp
            .UseLiveCharts().UseMauiCommunityToolkit(); // ✅ Ensure LiveCharts is properly initialized
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}