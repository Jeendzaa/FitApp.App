using Microsoft.Maui.Storage;
using FitApp.App.Views;
using FitApp.App.Services;

namespace FitApp.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

            if (isLoggedIn)
            {
                Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}
