using FitApp.App.ViewModels;
namespace FitApp.App.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Remove("UserId");
        Preferences.Remove("UserName");
        Preferences.Remove("UserEmail");
        Preferences.Remove("IsLoggedIn");

        await Shell.Current.GoToAsync("//LoginPage");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainPageViewModel vm)
            vm.LoadCommand.Execute(null);
    }
}

