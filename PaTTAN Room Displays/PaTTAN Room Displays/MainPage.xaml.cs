using System.Xml;
using System.Xml.Linq;
using System.ServiceModel.Syndication;

namespace PaTTAN_Room_Displays;

public partial class MainPage : ContentPage
{
	String URLString = "https://lancasterlebanon.resourcescheduler.net/rsevents/lobby_display.asp?StationID=1&ShowXML=1";

	public MainPage()
	{
		InitializeComponent();

        //Get the device name, and display it. Device names should be configured for the room that they represent.
        var deviceName = DeviceInfo.Name;
        RoomNameLabel.Text = deviceName;

        //Get the date and time and display them.
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

            if(roomName.Value == deviceName)
            {
                EventTitleLabel.Text = title.Value;
                EventTimeLabel.Text = startTime.Value + " - " + endTime.Value;
            }

            Console.WriteLine(title.Value);
            Console.WriteLine(startTime.Value);
            Console.WriteLine(endTime.Value);
            Console.WriteLine(contact.Value);
            Console.WriteLine(roomName.Value);


        }
        

    }
}

