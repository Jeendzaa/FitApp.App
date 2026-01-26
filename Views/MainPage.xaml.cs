namespace FitApp.App.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
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