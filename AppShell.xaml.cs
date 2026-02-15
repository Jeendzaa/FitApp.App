using FitApp.App.Views;

namespace FitApp.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false);
            Routing.RegisterRoute(nameof(MealsPage), typeof(MealsPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
    }
}
