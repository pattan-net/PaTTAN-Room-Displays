using System.Xml;
using System.ServiceModel.Syndication;
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;

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

    private void OnRefreshClick(object sender, EventArgs e)
    {
    	XElement full = XElement.Load(uri: URLString);
        IEnumerable<XElement> c1 = from el in full.Elements("channel").Elements("item") select el;
        Console.WriteLine("Begin of result set");
        foreach (XElement el in c1)
        {
        	XElement title = el.Descendants().Where(e => e.Name.LocalName == "title").FirstOrDefault();
                XElement startTime = el.Descendants().Where(e => e.Name.LocalName == "StartTime").FirstOrDefault();
                XElement endTime = el.Descendants().Where(e => e.Name.LocalName == "EndTime").FirstOrDefault();
                XElement contact = el.Descendants().Where(e => e.Name.LocalName == "Contact").FirstOrDefault();

                Console.WriteLine(title.Value);
                Console.WriteLine(startTime.Value);
                Console.WriteLine(endTime.Value);
                Console.WriteLine(contact.Value);

        }
        Console.WriteLine("End of result set");
    }
 
}

