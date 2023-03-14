namespace ApodTestApp;

public partial class App : Application
{
	public static bool UseHiDef { get; set; } = false;
	public static bool LoopAtEnd { get; set; } = true;

    // between 1 and 100
    public static int NumberOfRandomImages { get; set; } = 1;

    public static DateTime StartDate { get; set; } = DateTime.Now;
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override void OnStart()
    {
        base.OnStart();

        UseHiDef = Preferences.Default.Get("UseHiDef", false);
        NumberOfRandomImages = Preferences.Default.Get("NumberOfRandomImages", 1);

        // var start = Preferences.Default.Get("StartDate", DateTime.Now.ToBinary());
        StartDate = Preferences.Default.Get("StartDate", DateTime.Now);
        // StartDate = DateTime.FromBinary(start);
    }

    protected override void OnSleep()
    {
        base.OnSleep();

        Preferences.Default.Set("UseHiDef", UseHiDef);
        Preferences.Default.Set("NumberOfRandomImages", NumberOfRandomImages);

        var start = StartDate.ToBinary();
        Preferences.Default.Set("StartDate", start);

        int x = 0;

    }
}
