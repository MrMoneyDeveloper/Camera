using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using LibVLCSharp.Shared;
using Camera.CustomControls;
using Camera;
using Camera.Platforms.Windows;
using LibVLCSharp.MAUI;
using CommunityToolkit.Maui;

namespace Camera
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Core.Initialize(); // ✅ Ensures LibVLCSharp is initialized

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMediaElement()// ❌ Remove this
                .ConfigureMauiHandlers(handlers =>
                {
                    //handlers.AddHandler(typeof(VLCView), typeof(Camera.Platforms.Windows.VLCViewHandler));
                    handlers.AddHandler(typeof(VideoView), typeof(VLCViewHandler));
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseSkiaSharp(); // ✅ Integrates SkiaSharp Views
            


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
