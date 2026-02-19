using CommunityToolkit.Maui.Views;

namespace FitApp.App.Views.Popups;

public partial class CalendarPopup : Popup
{
    public DateTime SelectedDate => datePicker.Date;

    public CalendarPopup()
    {
        InitializeComponent();
        datePicker.Date = DateTime.Today;
    }

    private void OnConfirmClicked(object sender, EventArgs e)
    {
        Close(SelectedDate);
    }
}
