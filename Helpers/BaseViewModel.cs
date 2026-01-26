using CommunityToolkit.Mvvm.ComponentModel;

namespace FitApp.App.Helpers
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string title = string.Empty;
    }
}
