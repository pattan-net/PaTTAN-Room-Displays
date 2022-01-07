using System.Xml;
using System.ServiceModel.Syndication;

namespace PaTTAN_Room_Displays;

public partial class MainPage : ContentPage
{
	String URLString = "https://lancasterlebanon.resourcescheduler.net/rsevents/lobby_display.asp?StationID=1&ShowXML=1";

	public MainPage()
	{
		InitializeComponent();

		DateTimeLabel.Text = DateTime.Now.ToString("dddd, MMMM d h:mm tt");

        SyndicationFeed feed = null;

        try
        {
            using (var reader = XmlReader.Create(URLString))
            {
                feed = SyndicationFeed.Load(reader);
            }
        }
        catch 
        {
            Console.WriteLine("Did not load feed.");
        } 

        if (feed != null)
        {
            foreach (var element in feed.Items)
            {
                Console.WriteLine($"Title: {element.Title.Text}");
                Console.WriteLine($"Summary: {element.Summary.Text}");
            }
        }
    }

   
}

