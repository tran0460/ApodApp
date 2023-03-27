namespace ApodTestApp;

public partial class SlideShowPage : ContentPage
{
	public SlideShowPage()
	{
		InitializeComponent();
		SlideshowSlider.Value = Convert.ToDouble(App.SlideShowDelay);
	}

    private void SlideshowSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
		App.SlideShowDelay = Convert.ToInt32(SlideshowSlider.Value);

    }

    private async void SlideshowBtn_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}