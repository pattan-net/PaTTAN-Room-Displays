namespace PaTTAN_Room_Displays;

public partial class App : Application
{
	// configurable options
	int timerInteval = 5000; // in milliseconds 
	
	RoomDataService roomDataService = new RoomDataService();
	public static List<Meeting> meetingList = new List<Meeting>();
	DateTime LasteUpdated;
	private static System.Timers.Timer aTimer;
	int testTicks = 0;
	bool switchFlag = true;
	ContentPage hasMeetings = new MainPage();
	ContentPage hasNoMeetings = new NoMeetingsPage();
	ContentPage loadingDataPage = new LoadingDataPage();
	public App()
	{
		InitializeComponent();
		MainPage = loadingDataPage;
		ConfgiureUpdateDataTimer();
	}

	private void getData()
    {
		meetingList = roomDataService.GetRoomData();
    }

	private void ConfgiureUpdateDataTimer()
    {
		// Create a timer and set a two second interval.
		aTimer = new System.Timers.Timer();
		aTimer.Interval = timerInteval;
		// Hook up the Elapsed event for the timer. 
		aTimer.Elapsed += OnTimedEvent;
		// Have the timer fire repeated events (true is the default)
		aTimer.AutoReset = true;
		// Start the timer
		aTimer.Enabled = true;
	}

	private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
	{
		LasteUpdated = DateTime.UtcNow;
		meetingList = roomDataService.GetRoomData();
		testTicks += 1;
		if ( (testTicks%2) == 0 )
        {
			switchFlag = !switchFlag;

		}
		if (switchFlag)
		{
			this.MainPage = hasMeetings;
		}
		else
		{
			this.MainPage = hasNoMeetings;
		}
	}
}
