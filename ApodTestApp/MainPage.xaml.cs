using Microsoft.Maui.ApplicationModel.DataTransfer;
using System.Diagnostics;

namespace ApodTestApp;

public partial class MainPage : ContentPage
{
	private Apod apod = new();

	public string ImageDescription { get; set; } = string.Empty;

    private bool IsDescriptionShow = false;


    IDispatcherTimer timer = null;

	public MainPage()
	{
		InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);


        Uri uri = null;

        if (apod.LastUri == null)
        {
            uri = await apod.GetApodUri();
        }
        else
        {
            uri = apod.LastUri;
        }

        if (App.PreviousPage == "ChooseStartDate")
        {
            Loader.IsRunning = true;
            uri = await apod.GetApodUriByDate(App.StartDate);
        }

        if (App.PreviousPage == "SetRangeDate")
        {
            Loader.IsRunning = true;
            await apod.GetImagesByDateRange(App.DateRangeStartDate, App.DateRangeEndDate);
            uri = apod.ReturnCurrentImageInArrayUri();
        }

        if (App.PreviousPage == "PickRandomNumber")
        {
            Loader.IsRunning = true;
            await apod.GetNumberOfImages(App.NumberOfRandomImages);
            uri = apod.ReturnCurrentImageInArrayUri();
        }
        if (App.PreviousPage == "SlideshowPage")
        {
            StartTimer(App.SlideShowDelay);
        }
        if (uri != null)
        {
            Loader.IsRunning = false;
            TheImage.Source = uri;
            ImageDescription = apod.Information;
            Title = apod.Title;
            TheTitle.Text = apod.Date;
            ImageDescriptionLabel.Text = apod.Information;
            SemanticProperties.SetDescription(TheImage, apod.Information);
        }
    }

    private void OnSwiped(object sender, SwipedEventArgs e)
    {
        switch(e.Direction)
        {
            case SwipeDirection.Left:
                handleSwipeLeft();
                break;
            case SwipeDirection.Right:
                handleSwipeRight();
                break;
            case SwipeDirection.Down:
                StopTimer();
                App.ThePageBefore = "MainPage";
                break;
        }
    }

    private async void handleSwipeLeft()
    {
        if (App.PreviousPage == "ChooseStartDate" || App.PreviousPage == "MainPage" || (App.PreviousPage == "SlideshowPage" && App.ThePageBefore == "MainPage") || (App.PreviousPage == "SlideshowPage" && App.ThePageBefore == "ChooseStartDate"))
        {
            var prevUri = await apod.GetPreviousUri();
            TheImage.Source = prevUri;
        }
        if (App.PreviousPage == "SetRangeDate" || App.PreviousPage == "PickRandomNumber" || (App.PreviousPage == "SlideshowPage" && App.ThePageBefore == "SetRangeDate") || (App.PreviousPage == "SlideshowPage" && App.ThePageBefore == "PickRandomNumber"))
        {
            var prevUri = apod.ReturnNextImageInArrayUri();
            TheImage.Source = prevUri;
        }
        ImageDescription = apod.Information;
        Title = apod.Date;
        TheTitle.Text = apod.Date;
        ImageDescriptionLabel.Text = apod.Information;
        SemanticProperties.SetDescription(TheImage, apod.Information);
    }

    private async void handleSwipeRight()
    {
        // Additional conditions for slideshow handling
        if (App.PreviousPage == "ChooseStartDate" || App.PreviousPage == "MainPage" || (App.PreviousPage == "SlideshowPage" && App.ThePageBefore == "MainPage") || (App.PreviousPage == "SlideshowPage" && App.ThePageBefore == "ChooseStartDate"))
        {
            var prevUri = await apod.GetNextUri();
            TheImage.Source = prevUri;
        }
        // Additional conditions for slideshow handling
        else if (App.PreviousPage == "SetRangeDate" || App.PreviousPage == "PickRandomNumber" || (App.PreviousPage == "SlideshowPage" && App.ThePageBefore == "SetRangeDate") || (App.PreviousPage == "SlideshowPage" && App.ThePageBefore == "PickRandomNumber"))
        {
            var prevUri = apod.ReturnPreviousImageInArrayUri();
            TheImage.Source = prevUri;
        }
        ImageDescription = apod.Information;
        Title = apod.Date;
        TheTitle.Text = apod.Date;
        ImageDescriptionLabel.Text = apod.Information;
        SemanticProperties.SetDescription(TheImage, apod.Information);
    }

    private void StartTimer(int seconds)
    {
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(seconds);

        timer.Tick += (_, _) =>
        {
            handleSwipeLeft();
        };

        timer.Start();  
    }

    private void StopTimer() 
    {
        timer?.Stop(); 
    }

    private  void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
       
        ImageDescriptionLabel.IsVisible = !ImageDescriptionLabel.IsVisible;
        IsDescriptionShow = !IsDescriptionShow;
               
    }

    private async void settingsButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void shareImage_Clicked(object sender, EventArgs e)
    {
        if (TheImage.Source == null) return;
        IShare share = DependencyService.Get<IShare>();
        // If platform does not support sharing, return;
        Debug.WriteLine(share);
        if (share == null) return;  
        await ShareUri(TheImage.Source.ToString(), share);
    }

    public async Task ShareUri(string uri, IShare share)
    {
        await share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = "Check out this cool apod photo"
        });
    }

}

