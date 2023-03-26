namespace ApodTestApp;

public partial class PickRandomNumber : ContentPage
{
	public PickRandomNumber()
	{
		InitializeComponent();
        ImagesSlider.Value = Convert.ToDouble(App.NumberOfRandomImages);
        CurrentSliderValue.Text = $"{App.NumberOfRandomImages}";
	}

    private async void ChooseNumberOfImages_Clicked(object sender, EventArgs e)
    {
        App.PreviousPage = "PickRandomNumber";
        await Navigation.PushAsync(new MainPage());
    }

    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        App.NumberOfRandomImages = Convert.ToInt32(ImagesSlider.Value);
        CurrentSliderValue.Text = $"{App.NumberOfRandomImages}";

    }
}