﻿using System.Diagnostics;

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

        apod.GetApodUri(); // TODO - MOVE THIS

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
            uri = await apod.GetApodUriByDate(App.StartDate);
        }

        if (App.PreviousPage == "SetRangeDate")
        {
            await apod.GetImagesByDateRange(App.DateRangeStartDate, App.DateRangeEndDate);
            uri = apod.ReturnCurrentImageInArrayUri();
        }

        if (App.PreviousPage == "PickRandomNumber")
        {
            await apod.GetNumberOfImages(App.NumberOfRandomImages);
            uri = apod.ReturnCurrentImageInArrayUri();
        }
        if (uri != null)
        {
            TheImage.Source = uri;
            ImageDescription = apod.Information;
            Title = apod.Title;
            TheTitle.Text = apod.Date;
            ImageDescriptionLabel.Text = apod.Information;
        }
    }

    private async void OnSwiped(object sender, SwipedEventArgs e)
    {
        switch(e.Direction)
        {
            case SwipeDirection.Left:
                if (App.PreviousPage == "ChooseStartDate" || App.PreviousPage == "MainPage")
                {
                    var prevUri = await apod.GetPreviousUri();
                    TheImage.Source = prevUri;
                }
                if (App.PreviousPage == "SetRangeDate" || App.PreviousPage == "PickRandomNumber")
                {
                    var prevUri = apod.ReturnNextImageInArrayUri();
                    TheImage.Source = prevUri;
                }
                ImageDescription = apod.Information;
                Title = apod.Date;
                TheTitle.Text = apod.Date;
                ImageDescriptionLabel.Text = apod.Information;
                break;
            case SwipeDirection.Right:
                if (App.PreviousPage == "ChooseStartDate" || App.PreviousPage == "MainPage")
                {
                    var prevUri = await apod.GetNextUri();
                    TheImage.Source = prevUri;
                }
                else if (App.PreviousPage == "SetRangeDate" || App.PreviousPage == "PickRandomNumber")
                {
                    var prevUri = apod.ReturnPreviousImageInArrayUri();
                    TheImage.Source = prevUri;
                }
                ImageDescription = apod.Information;
                Title = apod.Date;
                TheTitle.Text = apod.Date;
                ImageDescriptionLabel.Text = apod.Information;
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

    private  void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
       
        ImageDescriptionLabel.IsVisible = !ImageDescriptionLabel.IsVisible;
        IsDescriptionShow = !IsDescriptionShow;
               
    }

    private async void settingsButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}

