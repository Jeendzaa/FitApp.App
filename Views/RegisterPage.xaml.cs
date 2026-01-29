using FitApp.App.ViewModels;

namespace FitApp.App.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(RegisterViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
