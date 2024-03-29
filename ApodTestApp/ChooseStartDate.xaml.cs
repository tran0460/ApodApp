using System.Diagnostics;

namespace ApodTestApp;

public partial class ChooseStartDate : ContentPage
{
	public ChooseStartDate()
	{
        InitializeComponent();
        StartDatePicker.Date = App.StartDate;
        StartDatePicker.MaximumDate = DateTime.Today;
	}

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {   
        App.StartDate = StartDatePicker.Date;

    }

    private async void ChooseStartDateBtn_Clicked(object sender, EventArgs e)
    {
        App.PreviousPage = "ChooseStartDate";

        await Shell.Current.GoToAsync("//MainPage");

    }
}