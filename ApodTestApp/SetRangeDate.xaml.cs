namespace ApodTestApp;

public partial class SetRangeDate : ContentPage
{
	public SetRangeDate()
	{
		InitializeComponent();
        StartDatePicker.Date = App.DateRangeStartDate;
        EndDatePicker.Date = App.DateRangeEndDate;
        StartDatePicker.MaximumDate = DateTime.Now;
        EndDatePicker.MaximumDate = DateTime.Now;
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