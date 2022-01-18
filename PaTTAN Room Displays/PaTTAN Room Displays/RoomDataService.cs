using System;
using System.Reflection;
using System.Xml.Linq;

namespace PaTTAN_Room_Displays
{
    public class RoomDataService
    {
        private DateTime lastUpdated;
        public DateTime LastUpdated {
            get
            {
                return lastUpdated;
            }
            set {
                lastUpdated = TimeZoneInfo.ConvertTimeFromUtc(value, App.easternZone);
            }
        }

        public RoomDataService()
        {
            LastUpdated = DateTime.UtcNow;
        }

        public List<Meeting> GetRoomData()
        {
            List<Meeting> MeetingList = new List<Meeting>();
#if RELEASE
            String URLString = "https://lancasterlebanon.resourcescheduler.net/rsevents/lobby_display.asp?StationID=1&ShowXML=1";
#endif
#if DEBUG
            // on windows this is C:\users\USERNAME\AppData\local
            // For testing the timer and room service app you can open this file with notepad while the app is running and make edits.
            String URLString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "roomtest.xml");
#endif

            XElement meetingRoomDataAll = XElement.Load(URLString);
            IEnumerable<XElement> meetingItems = from el in meetingRoomDataAll.Elements("channel").Elements("item") select el;
            Console.WriteLine("Begin of result set");
            String dateTimePattern = "MMM d, yyyy h:mm tt";
            foreach (XElement el in meetingItems)
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
                MeetingList.Add(temp);
            }
            LastUpdated = DateTime.UtcNow;
            return MeetingList;
        }
    }
}
