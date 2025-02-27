namespace EcommerceApp.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfilePageModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ProfilePageModel)BindingContext).InitializeAsync();
    }
}