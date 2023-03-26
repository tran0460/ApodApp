namespace ApodTestApp;

public partial class App : Application
{
    public static bool UseHiDef { get; set; } = false;
    public static bool LoopAtEnd { get; set; } = true;

    // between 1 and 100
    public static int NumberOfRandomImages { get; set; } = 1;

    public static int SlideShowDelay { get; set; } = 1;

    public static DateTime StartDate { get; set; } = DateTime.Now;

    public static DateTime DateRangeStartDate { get; set; } = DateTime.Now;
    public static DateTime DateRangeEndDate { get; set; } = DateTime.Now;

    // Previous page the user was on
    public static string PreviousPage { get; set; } = "MainPage";
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override void OnStart()
    {
        base.OnStart();
        //TODO: add slide show delay

        StartDate = Preferences.Default.Get("StartDate", DateTime.Now);
        UseHiDef = Preferences.Default.Get("UseHiDef", false);
        NumberOfRandomImages = Preferences.Default.Get("NumberOfRandomImages", 1);
        SlideShowDelay = Preferences.Default.Get("SlideShowDelay", 1);
        DateRangeStartDate = Preferences.Default.Get("DateRangeStartDate", DateTime.Now);
        DateRangeEndDate = Preferences.Default.Get("DateRangeEndDate", DateTime.Now);

    }

    protected override void OnSleep()
    {
        base.OnSleep();
        //TODO: add slide show delay

        Preferences.Default.Set("UseHiDef", UseHiDef);
        Preferences.Default.Set("NumberOfRandomImages", NumberOfRandomImages);
        Preferences.Default.Set("SlideShowDelay", SlideShowDelay);

        var start = StartDate.ToBinary();
        Preferences.Default.Set("StartDate", start);
        var dateRangeStart = DateRangeStartDate.ToBinary();
        var dateRangeEnd = DateRangeEndDate.ToBinary();
        Preferences.Default.Set("DateRangeStartDate", DateRangeStartDate);
        Preferences.Default.Set("DateRangeEndDate", DateRangeEndDate);

    }
}
