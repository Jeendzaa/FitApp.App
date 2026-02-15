using FitApp.App.Services;
using Microsoft.Extensions.Logging;
using FitApp.App.Views;
using FitApp.App.ViewModels;
using CommunityToolkit.Maui;

namespace FitApp.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("JaroRegular.ttf", "JaroRegular");
                })
                .UseMauiCommunityToolkit();

            builder.Services.AddHttpClient<UserService>();
            builder.Services.AddHttpClient<DailyService>();

            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<RegisterViewModel>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<RegisterPage>();

            builder.Services.AddSingleton<MealService>();
            builder.Services.AddSingleton<MealEntryService>();
            builder.Services.AddTransient<MealsPageViewModel>();
            builder.Services.AddTransient<MealsPage>();
            builder.Services.AddTransient<SettingsPageViewModel>();
            builder.Services.AddTransient<SettingsPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
