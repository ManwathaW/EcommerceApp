using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace EcommerceApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            try
            {
                InitializeComponent();

                this.SetValue(Microsoft.Maui.Controls.Shell.TabBarBackgroundColorProperty, Color.FromArgb("#FFF9E6"));
                this.SetValue(Microsoft.Maui.Controls.Shell.TabBarUnselectedColorProperty, Color.FromArgb("#000000"));
                this.SetValue(Microsoft.Maui.Controls.Shell.TabBarForegroundColorProperty, Color.FromArgb("#FFFF00"));

                // Register routes for navigation
                Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
                Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
                Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AppShell initialization error: {ex.Message}");
              
            }
        }

    }
}
