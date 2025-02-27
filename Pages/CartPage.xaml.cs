namespace EcommerceApp.Pages;

public partial class CartPage : ContentPage
{
   
        public CartPage(NavigationService navigationService, CartPageModel viewModel)
        {
            InitializeComponent();
            navigationService.Initialize(Navigation);
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ((CartPageModel)BindingContext).InitializeAsync();
        }
    
}