﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaTTAN_Room_Displays
{
    internal class Meeting
    {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public String title { get; set; }
        public String roomName { get; set; }
        public String contact { get; set; }

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
