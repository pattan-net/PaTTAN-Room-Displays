using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaTTAN_Room_Displays
{
    internal class Meeting
    {
        DateTime startTime { get; set; }
        DateTime endTime { get; set; }
        String title { get; set; }
        String roomName { get; set; }
        String contact { get; set; }

        public Meeting(String inContact, String inRoomName, String inTitle, DateTime inStartTime, DateTime inEndtime)
        {
            startTime = inStartTime;
            endTime = inEndtime;
            title = inTitle;
            roomName = inRoomName;
            contact = inContact;
        }
    }
}
