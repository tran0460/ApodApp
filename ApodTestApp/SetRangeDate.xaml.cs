namespace ApodTestApp;

public partial class SetRangeDate : ContentPage
{
	public SetRangeDate()
	{
		InitializeComponent();
	}

    private async void ChooseDateRange_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private void StartDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        App.DateRangeStartDate = StartDatePicker.Date;
    }

    private void EndDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        App.DateRangeEndDate = EndDatePicker.Date;
    }
}