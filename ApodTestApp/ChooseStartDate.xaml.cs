using System.Diagnostics;

namespace ApodTestApp;

public partial class ChooseStartDate : ContentPage
{
    private DateTime startDate;
	public ChooseStartDate()
	{
        InitializeComponent();
	}


    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {   
        App.StartDate = StartDatePicker.Date;
    }

    private async void ChooseStartDateBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());

    }
}