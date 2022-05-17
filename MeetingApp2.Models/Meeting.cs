using System;
using System.Collections.Generic;

namespace MeetingApp2.Models
{
    public class Meeting
    {
        public List<string> Participants { get; set; }
        public string Name { get; set; }
        public string ResponsiblePerson { get; set; } //Veliau galima pakeisti i klase Person
        public string Description { get; set; } 
        public MeetingCategory Category { get; set; }
        public MeetingType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Meeting()
        {
            Participants = new List<string>(); 
        }

    }
    public enum MeetingCategory
    {
        CodeMonkey, 
        Hub, 
        Short, 
        TeamBuilding
    }
    public enum MeetingType
    {
        Live,
        InPerson
    }

}
