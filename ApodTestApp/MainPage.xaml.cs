using System.Diagnostics;

namespace ApodTestApp;

public partial class MainPage : ContentPage
{
	private Apod apod = new();

	public string ImageDescription { get; set; } = string.Empty;


    IDispatcherTimer timer = null;

	public MainPage()
	{
		InitializeComponent();

		//apod.GetApodUri(); // TODO - MOVE THIS

	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Debug.WriteLine(App.PreviousPage);

        Uri uri = null;

        Debug.WriteLine(args);

        if (apod.LastUri == null)
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

    private void StartTimer(int seconds)
    {
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(seconds);

        timer.Tick += async (_, _) =>
        {
            //TheImage.IsVisible = !TheImage.IsVisible;
            var prevUri = await apod.GetPreviousUri();
            TheImage.Source = prevUri;
            ImageDescription = apod.Information;
            Title = apod.Date;
        };

        timer.Start();
    }

    private void StopTimer() 
    {
        timer?.Stop(); 
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

