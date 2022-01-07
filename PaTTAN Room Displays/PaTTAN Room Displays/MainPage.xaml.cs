using System.Xml.Linq;

namespace PaTTAN_Room_Displays;

public partial class MainPage : ContentPage
{
    //Lobby display RSS feed for Resource Scheduler. The Lobby feed will have all events for the day, so we can parse it for a particular room.
	String URLString = "https://lancasterlebanon.resourcescheduler.net/rsevents/lobby_display.asp?StationID=1&ShowXML=1";

	public MainPage()
	{
		InitializeComponent();

        //Get the mobile device name, and display it. Device names should be configured for the room that they represent.
        var deviceName = DeviceInfo.Name;
        RoomNameLabel.Text = deviceName;

        //Get the date and time and display them.

        //The code below doesn't work in iOS for reasons that I have yet to determine.
        //var timeUtc = DateTime.UtcNow;
        //var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        //var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

        DateTimeLabel.Text = DateTime.Now.ToString("dddd, MMMM d | h:mm tt");

        XElement full = XElement.Load(uri: URLString);
        IEnumerable<XElement> c1 = from el in full.Elements("channel").Elements("item") select el;
        Console.WriteLine("Begin of result set");
        foreach (XElement el in c1)
        {
            XElement roomName = el.Descendants().Where(e => e.Name.LocalName == "RoomName").FirstOrDefault();
            XElement title = el.Descendants().Where(e => e.Name.LocalName == "title").FirstOrDefault();
            XElement startTime = el.Descendants().Where(e => e.Name.LocalName == "StartTime").FirstOrDefault();
            XElement endTime = el.Descendants().Where(e => e.Name.LocalName == "EndTime").FirstOrDefault();
            XElement contact = el.Descendants().Where(e => e.Name.LocalName == "Contact").FirstOrDefault();

            //Check if any feed elements contain event information for the room that this device is assigned to.
            //If so, display the event title and start/end times.
            if(roomName.Value == deviceName)
            {
                EventTitleLabel.Text = title.Value;
                EventTimeLabel.Text = startTime.Value + " - " + endTime.Value;
            }
        }
    }
}

