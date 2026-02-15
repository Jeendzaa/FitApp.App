using FitApp.App.ViewModels;

namespace FitApp.App.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Remove("UserId");
        Preferences.Remove("UserName");
        Preferences.Remove("UserEmail");
        Preferences.Remove("IsLoggedIn");

        await Shell.Current.GoToAsync("//LoginPage");
    }
}
