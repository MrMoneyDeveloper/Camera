using Camera.Pages;
using Microsoft.Maui.Controls;

namespace Camera // ✅ Ensure this matches AppShell.xaml
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent(); // ✅ This should now work

            Routing.RegisterRoute(nameof(MainPage), typeof(Camera.Pages.MainPage));
            Routing.RegisterRoute(nameof(CameraSettingsPage), typeof(Camera.Pages.CameraSettingsPage));
            Routing.RegisterRoute(nameof(RTSPStreamPage), typeof(Camera.Pages.RTSPStreamPage));

        }
    }
}

