using MeetingApp2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MeetingApp2
{
    internal class Program
    {
        private const string InputFile = "Input/Meetings.json";
        public static List<Meeting> Meetings;

        static void Main(string[] args)
        {
            Meetings = JsonConvert.DeserializeObject<List<Meeting>>(File.ReadAllText(InputFile));

            Console.WriteLine("Choose a desired option:");
            var chosenOption = Console.ReadLine();
            switch (chosenOption)
            {
                case "Create":
                    AddMeeting();
                    break;
                case "Delete":
                    break;
                case "AddPerson":
                    break;
                case "RemovePerson":
                    break;
                case "DisplayListMeetings":
                    break;
                default:
                    Console.WriteLine("Enter again");
                    break;
            }
            //Komandu nuskaitymas - vykdymas
            File.WriteAllText(InputFile, JsonConvert.SerializeObject(Meetings));
            return;
        }

        public static void AddMeeting()
        {
            var meeting = new Meeting();
            meeting.Name = Console.ReadLine();
            meeting.Description = Console.ReadLine();
            meeting.ResponsiblePerson = Console.ReadLine();

            var categoryString = Console.ReadLine();
            MeetingCategory category;
            while (!Enum.TryParse(categoryString, out category))
            {
                categoryString = Console.ReadLine();
            }

            meeting.Category = category;

            var typeString = Console.ReadLine(); ;
            MeetingType type;
            while (!Enum.TryParse(typeString, out type))
            {
                typeString = Console.ReadLine();
            }

            meeting.Type = type;

            var startDateString = Console.ReadLine();
            DateTime startDate;
            while (!DateTime.TryParse(startDateString, out startDate))
            {
                startDateString = Console.ReadLine();
            }

            meeting.StartDate = startDate;

            var endDateString = Console.ReadLine();
            DateTime endDate;
            while (!DateTime.TryParse(endDateString, out endDate))
            {
                endDateString = Console.ReadLine();
            }

            meeting.EndDate = endDate;

            Meetings.Add(meeting);
        }

        public static IQueryable<Meeting> GetMeetings(string responsiblePerson = null, string meetingName = null, string description = null,
                                                MeetingCategory? category = null, MeetingType? type = null, DateTime? startDate = null,
                                                DateTime? endDate = null, int? attendeeCount = null)
        {
            var query = Meetings.AsQueryable();

            if (responsiblePerson != null)
            {
                query = query.Where(query => query.ResponsiblePerson == responsiblePerson);
            }
            if (meetingName != null)
            {
                query = query.Where(query => query.Name.Equals(meetingName, StringComparison.CurrentCultureIgnoreCase));
            }
            if (description != null)
            {
                query = query.Where(query => query.Description.Contains(description, StringComparison.CurrentCultureIgnoreCase));
            }
            if (category.HasValue)
            {
                query = query.Where(query => query.Category == category.Value);
            }
            if (type.HasValue)
            {
                query = query.Where(query => query.Type == type.Value);
            }
            if (startDate.HasValue)
            {
                query = query.Where(query => query.StartDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(query => query.EndDate <= endDate.Value);
            }
            if (attendeeCount.HasValue)
            {
                query = query.Where(query => query.Participants != null && query.Participants.Count() >= attendeeCount.Value);
            }

            return query;
        }

        public static void DeleteMeeting()
        {
            var responsiblePerson = Console.ReadLine();
            var meetingName = Console.ReadLine();
            var deletableMeeting = GetMeetings(responsiblePerson, meetingName).FirstOrDefault();

            if (deletableMeeting != null)
            {
                Meetings.Remove(deletableMeeting);
            }
        }

        public static void AddPerson()
        {
            /*● Command to add a person to the meeting.
                ○ Command should specify who is being added and at what time.
                ○ If a person is already in a meeting which intersects with the one being added,
                  a warning message should be given.
                ○ Prevent the same person from being added twice. */
        }

        public static void RemovePerson()
        {
            var meetingName = Console.ReadLine();
            var deletablePerson = Console.ReadLine();
            var modifiableMeeting = GetMeetings(meetingName: meetingName).FirstOrDefault();
            
            if (modifiableMeeting != null && 
                modifiableMeeting.ResponsiblePerson != deletablePerson && 
                modifiableMeeting.Participants.Contains(deletablePerson))
            {
                modifiableMeeting.Participants.Remove(deletablePerson);
            }
        }

        public static void DisplayMeetings()
        {
            string responsiblePerson = null;
            string description = null;
            MeetingCategory? category = null;
            MeetingType? type = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            int? attendeeCount = null;

            var parameterName = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(parameterName))
            {
                switch (parameterName)
                {
                    case "responsiblePerson":
                        responsiblePerson = Console.ReadLine();
                        break;

                        case "description": 
                        description = Console.ReadLine(); 
                        break;

                        case "category":
                        if (!Enum.TryParse(Console.ReadLine(), out MeetingCategory categoryRes))
                        {
                            category = categoryRes;
                        }
                        break;

                        case "type":
                        if (!Enum.TryParse(Console.ReadLine(), out MeetingType typeRes))
                        {
                            type = typeRes;
                        }
                        break;

                        case "startDate":                        
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDateRes))
                        {
                            startDate = startDateRes;
                        }
                        break;

                        case "endDate":
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDateRes))
                        {
                            endDate = endDateRes;
                        }
                        break;

                }

            }
            /*● Command to list all the meetings. Add the following parameters to filter the data:
                ○ Filter by description (if the description is “Jono .NET meetas”, searching for
                    .NET should return this entry)
                    ○ Filter by responsible person
                    ○ Filter by category
                    ○ Filter by type
                    ○ Filter by dates (e.g meetings that will happen starting from 2022-01-01 /
                        meetings that will happen between 2022-01-01 and 2022-02-01)
                    ○ Filter by the number of attendees (e.g show meetings that have over 10
                        people attending) */
        }
    }
}
