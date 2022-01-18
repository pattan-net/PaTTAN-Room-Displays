using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;

namespace PaTTAN_Room_Displays
{
    internal class RoomDataService
    {
        private DateTime _lastUpdated;
        public DateTime lastUpdated {
            get
            {
                return _lastUpdated;
            }
            set {
                _lastUpdated = TimeZoneInfo.ConvertTimeFromUtc(value, App.easternZone);
            }
        }

        public RoomDataService()
        {
            lastUpdated = DateTime.UtcNow;
        }

        public List<Meeting> GetRoomData()
        {
            List<Meeting> MeetingList = new List<Meeting>(); 
            #if RELEASE
                String URLString = "https://lancasterlebanon.resourcescheduler.net/rsevents/lobby_display.asp?StationID=1&ShowXML=1";
            #endif
            #if DEBUG
                    String xmlFileName = "PaTTAN_Room_Displays.Resources.threeMeetingsInDifferentRooms.xml";
                    // String xmlFileName = "PaTTAN_Room_Displays.Resources.twoMeetingsOneDay.xml";
                    // String xmlFileName = "PaTTAN_Room_Displays.Resources.noMeetings.xml";
                    var assembly = typeof(App).GetTypeInfo().Assembly;
                    Stream URLString = assembly.GetManifestResourceStream(xmlFileName);
            #endif

            //Get and parse XML from resource scheduler
            XElement full = XElement.Load(URLString);
            IEnumerable<XElement> c1 = from el in full.Elements("channel").Elements("item") select el;
            // The data in the xml comes in two different elements.  This pattern is used to convert those elements to a DateTime object
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
                MeetingList.Add(temp);
            }
            lastUpdated = DateTime.UtcNow;
            return MeetingList;
        }
    }
}
