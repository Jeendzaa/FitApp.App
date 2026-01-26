using FitApp.App.ViewModels;

namespace FitApp.App.Views;
public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}