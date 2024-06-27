namespace DocPlanner.Client.Models
{
    public class AvailabilityResponse
    {

        public Facility Facility { get; set; }
        public int SlotDurationMinutes { get; set; }
        public DaySchedule Monday { get; set; }
        public DaySchedule Tuesday { get; set; }
        public DaySchedule Wednesday { get; set; }
        public DaySchedule Thrusday { get; set; }
        public DaySchedule Friday { get; set; }
    }

    public class Facility
    {
        public Guid FacilityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class DaySchedule
    {
        public WorkPeriod WorkPeriod { get; set; }
        public IEnumerable<BusySlot> BusySlots { get; set; } = new List<BusySlot>();
    }

    public class WorkPeriod
    {
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int LunchStartHour { get; set; }
        public int LunchEndHour { get; set; }
    }

    public class BusySlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
