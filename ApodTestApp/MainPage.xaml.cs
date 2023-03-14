namespace ApodTestApp;

public partial class MainPage : ContentPage
{
	private Apod apod = new();

	public string ImageDescription { get; set; } = string.Empty;



	public MainPage()
	{
		InitializeComponent();

		apod.GetApodUri(); // TODO - MOVE THIS


	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Uri uri = null;

        if(apod.LastUri == null)
        {
            uri = await apod.GetApodUri();
        }
        else
        {
            uri = apod.LastUri;
        }

        if(uri != null)
        {
            TheImage.Source = uri;
            ImageDescription = apod.Information;
            Title = apod.Title;
        }
    }

    private async void OnSwiped(object sender, SwipedEventArgs e)
    {
        switch(e.Direction)
        {
            case SwipeDirection.Left:
                var prevUri = await apod.GetPreviousUri();
                TheImage.Source = prevUri;
                ImageDescription = apod.Information;
                Title = apod.Date;
                break;
            case SwipeDirection.Right:
                var uri = await apod.GetNextUri();
                TheImage.Source = uri;
                ImageDescription = apod.Information;
                Title = apod.Date;
                break;
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Image Description", ImageDescription, "OK");
    }

    private async void settingsButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}

