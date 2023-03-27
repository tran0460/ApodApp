namespace ApodTestApp;

public partial class SlideShowPage : ContentPage
{
	public SlideShowPage()
	{
		InitializeComponent();
		SlideshowSlider.Value = Convert.ToDouble(App.SlideShowDelay);
        DelayValue.Text = $"{App.SlideShowDelay}";
	}

    private void SlideshowSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
		App.SlideShowDelay = Convert.ToInt32(SlideshowSlider.Value);
        DelayValue.Text = $"{App.SlideShowDelay}";
    }

    private async void SlideshowBtn_Clicked(object sender, EventArgs e)
    {
        App.ThePageBefore = App.PreviousPage;
        App.PreviousPage = "SlideshowPage";

        await Shell.Current.GoToAsync("//MainPage");
    }
}