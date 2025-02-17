using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace EcommerceApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            this.SetValue(Microsoft.Maui.Controls.Shell.TabBarBackgroundColorProperty, Color.FromArgb("#262421")); // dark theme background
            this.SetValue(Microsoft.Maui.Controls.Shell.TabBarUnselectedColorProperty, Color.FromArgb("#FFFFFF")); // Unselected white
            this.SetValue(Microsoft.Maui.Controls.Shell.TabBarForegroundColorProperty, Color.FromArgb("#FFFF00")); //  primary Yellow
            // Register routes for navigation
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));


        }
       
    }
}
