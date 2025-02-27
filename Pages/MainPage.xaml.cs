
using EcommerceApp.PageModels;
using Syncfusion.Maui.Toolkit.Carousel;

namespace EcommerceApp.Pages
{
 

    public partial class MainPage : ContentPage
    { 
        private readonly MainPageModel _model;
        public MainPage(NavigationService navigationService,MainPageModel model)
        {
            InitializeComponent();
            _model = model;
            navigationService.Initialize(Navigation);
            BindingContext = model;
        }
    }
}
