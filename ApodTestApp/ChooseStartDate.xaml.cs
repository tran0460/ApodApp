using System.Diagnostics;

namespace ApodTestApp;

public partial class ChooseStartDate : ContentPage
{
	public ChooseStartDate()
	{
		InitializeComponent();
	}


    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        App.StartDate = StartDatePicker.Date;
        Debug.WriteLine(App.StartDate);
    }

    private async void ChooseStartDateBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());

    }
}