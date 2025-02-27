using System;
using CommunityToolkit.Mvvm.ComponentModel;
using EcommerceApp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.PageModels
{
    public partial class BasePageModel : ObservableObject
    {
        protected readonly DatabaseContext Database;
        private INavigation _navigation;

        // Add property to set the navigation after initialization
        public INavigation Navigation
        {
            get => _navigation;
            set => _navigation = value;
        }

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private bool isRefreshing;

        public BasePageModel(DatabaseContext database)
        {
            Database = database;
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;

        protected async Task ExecuteBusyActionAsync(Func<Task> action, bool showBusy = true)
        {
            if (showBusy)
                isBusy = true;

            try
            {
                await action();
            }
            finally
            {
                if (showBusy)
                    isBusy = false;
            }
        }
    }
}
