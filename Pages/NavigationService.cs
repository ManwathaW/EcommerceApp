using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Pages
{
    public class NavigationService
    {
        private INavigation _navigation;

        public void Initialize(INavigation navigation)
        {
            _navigation = navigation;
        }

        public Task NavigateToAsync<T>(object parameter = null) where T : Page
        {
            try
            {
                // Get the page from the service provider
                var page = Activator.CreateInstance(typeof(T));
                return _navigation.PushAsync(page as Page);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
                return Task.CompletedTask;
            }
        }
    }
}
