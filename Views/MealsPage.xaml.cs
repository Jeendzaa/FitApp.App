using FitApp.App.ViewModels;

namespace FitApp.App.Views;

public partial class MealsPage : ContentPage
{
    public MealsPage(MealsPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
