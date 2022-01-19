using TimeZoneConverter;

namespace PaTTAN_Room_Displays;

public partial class MainPage : ContentPage
{
    //Lobby display RSS feed for Resource Scheduler. The Lobby feed will have all events for the day, so we can parse it for a particular room.
    List<Meeting> meetings = new List<Meeting>();

    public MainPage()
    {
        InitializeComponent();

#if RELEASE
        String deviceName = DeviceInfo.Name;
#endif
#if DEBUG
        String deviceName = "Meeting Room 1";
#endif

        //Get the mobile device name, and display it. Device names should be configured for the room that they represent.
        RoomNameLabel.Text = deviceName;
        //Get the date and time and display them.
        //Windows and Linux perform time zone lookup in different databases as described below. TimeZoneConverter package installed to resolve this.
        // https://devblogs.microsoft.com/dotnet/cross-platform-time-zones-with-net-core/

        var timeUtc = DateTime.UtcNow;
        TimeZoneInfo easternZone = TZConvert.GetTimeZoneInfo("America/New_York");
        var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
        DateTimeLabel.Text = today.ToString("dddd, MMMM d | h:mm tt");


        // There are not that many meetings in a day.  We only put meetings on the list that haven't ended.
        // Later on we just show the top of the list.
        foreach (Meeting meeting in App.meetingList)
        {
            if (today.TimeOfDay < meeting.startTime.TimeOfDay || today.TimeOfDay < meeting.endTime.TimeOfDay)
            {
                meetings.Add(meeting);
            }
        }

        //@toddo sor the list by start time.

        EventTitleLabel.Text = meetings[0].title;
        EventTimeLabel.Text = meetings[0].startTime.ToString("HH:mm tt");

    }
}

