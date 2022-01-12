using System.Xml.Linq;
using TimeZoneConverter;
using System.Reflection;

namespace PaTTAN_Room_Displays;

public partial class MainPage : ContentPage
{
    //Lobby display RSS feed for Resource Scheduler. The Lobby feed will have all events for the day, so we can parse it for a particular room.
    List<Meeting> meetingList = new List<Meeting>();

    public MainPage()
	{
		InitializeComponent();
#if RELEASE
    String URLString = "https://lancasterlebanon.resourcescheduler.net/rsevents/lobby_display.asp?StationID=1&ShowXML=1";
    String deviceName = DeviceInfo.Name;
#endif
#if DEBUG
        // String xmlFileName = "PaTTAN_Room_Displays.Resources.twoMeetingsOneDay.xml";
        // String xmlFileName = "PaTTAN_Room_Displays.Resources.twoMeetingsOneDay.xml";
        String xmlFileName = "PaTTAN_Room_Displays.Resources.threeMeetingsInDifferentRooms.xml";
        var assembly = typeof(App).GetTypeInfo().Assembly;
        Stream URLString = assembly.GetManifestResourceStream(xmlFileName);
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
        
        XElement full = XElement.Load(URLString);
        IEnumerable<XElement> c1 = from el in full.Elements("channel").Elements("item") select el;
        Console.WriteLine("Begin of result set");
        String dateTimePattern = "MMM d, yyyy h:mm tt";
        foreach (XElement el in c1)
        {
            XElement roomName = el.Descendants().Where(e => e.Name.LocalName == "RoomName").FirstOrDefault();
            XElement title = el.Descendants().Where(e => e.Name.LocalName == "title").FirstOrDefault();
            XElement startTime = el.Descendants().Where(e => e.Name.LocalName == "StartTime").FirstOrDefault();
            XElement startDate = el.Descendants().Where(e => e.Name.LocalName == "StartDate").FirstOrDefault();
            XElement endDate = el.Descendants().Where(e => e.Name.LocalName == "EndDate").FirstOrDefault();
            XElement endTime = el.Descendants().Where(e => e.Name.LocalName == "EndTime").FirstOrDefault();
            XElement contact = el.Descendants().Where(e => e.Name.LocalName == "Contact").FirstOrDefault();
            // good doc on date manipulations https://docs.microsoft.com/en-us/dotnet/api/system.datetime?view=net-6.0#initialization-04
            Meeting temp = new Meeting(
                contact.Value, 
                roomName.Value, 
                title.Value, 
                DateTime.ParseExact(startDate.Value + " " + startTime.Value, dateTimePattern, null), 
                DateTime.ParseExact(endDate.Value + " " + endTime.Value, dateTimePattern, null)
            );
            // There are not that many meetings in a day.  We only put meetings on the list that haven't ended.
            // Later on we just show the top of the list. 
            if(today.TimeOfDay < temp.startTime.TimeOfDay || today.TimeOfDay < temp.endTime.TimeOfDay)
            {
                meetingList.Add(temp);
            }
        }
        //@toddo sor the list by start time. 

        EventTitleLabel.Text = meetingList[0].title;
        EventTimeLabel.Text = meetingList[0].startTime.ToString("HH:mm tt");
        
    }
}

