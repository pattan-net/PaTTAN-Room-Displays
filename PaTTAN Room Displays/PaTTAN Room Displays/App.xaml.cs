using TimeZoneConverter;

namespace PaTTAN_Room_Displays;

public partial class App : Application
{
	/// <summary>
	/// Class <c>App</c>:
	/// Entry point for the room display application.  
	/// This app is designed to run on a mobile device that is hung outside a of a conference room 
	/// to display details about the current meeting.
	/// </summary>
	// configurable options
	readonly int RefreshRoomDataTimeInterval = 5000; // in milliseconds should be 5 min
	public static TimeZoneInfo easternZone = TZConvert.GetTimeZoneInfo("America/New_York");
	#if RELEASE
		public static String deviceName = DeviceInfo.Name;
	#endif
	#if DEBUG
		public static String deviceName = "Meeting Room 1";
	#endif

	RoomDataService roomDataService = new RoomDataService();
	public static List<Meeting> meetingList = new List<Meeting>(); // List of meetings for room defined by deviceName 
	private static System.Timers.Timer dataRefreshTimer; // Timer object used to repeatedly retreive room data see dataRefreshTimerCallback for details.

	// Various application pages 
	ContentPage hasMeetings = new MainPage();
	ContentPage hasNoMeetings = new NoMeetingsPage();
	ContentPage loadingDataPage = new LoadingDataPage();

	public App()
	{
		InitializeComponent();
		MainPage = loadingDataPage;
		ConfigureUpdateDataTimer();
	}

	private void ConfigureUpdateDataTimer()
    {
		dataRefreshTimer = new System.Timers.Timer();
		dataRefreshTimer.Interval = RefreshRoomDataTimeInterval;
		// Hook up the Elapsed event for the timer. 
		dataRefreshTimer.Elapsed += dataRefreshTimerCallback;
		// Have the timer fire repeated events (true is the default)
		dataRefreshTimer.AutoReset = true;
		// Start the timer
		dataRefreshTimer.Enabled = true;
	}

	private void dataRefreshTimerCallback(Object source, System.Timers.ElapsedEventArgs e)
	{
		///<summary>
		/// Timmer callback. Repsonsible for getting room data, post procssing, and setting active page display. 
		/// </summary>
		meetingList = roomDataService.GetRoomData();
		trimRoomDataByDeviceName();
		setPage();
		System.Diagnostics.Debug.WriteLine(roomDataService.LastUpdated);
	}

	private void trimRoomDataByDeviceName()
    {
		///<summary>
		/// Remove any meeting data other than for the spcified room.  Define room by setting the devices name. 
		/// </summary>
		for(int i =0; i<meetingList.Count; ++i) 
        {
			if(meetingList[i].roomName != deviceName)
            {
				meetingList.RemoveAt(i);
            }
        }
    }

	private void setPage()
    {
		///<summary>
		/// Sets the application page based on data found in the meeting list.  
		/// </summary>
		Dispatcher.Dispatch(
		new Action(() => {
			if (meetingList.Count > 0 )
			{
				MainPage = hasMeetings;
			}
			else
			{
				MainPage = hasNoMeetings;
			}
		}));
	}
}
