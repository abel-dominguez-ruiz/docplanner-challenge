namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public class WeeklyAvailabilityModel
    {

        public FacilityModel Facility { get; set; }
        public int SlotDurationMinutes { get; set; }


        public DayScheduleModel Monday { get; set; }
        public DayScheduleModel Tuesday { get; set; }
        public DayScheduleModel Wednesday { get; set; }
        public DayScheduleModel Thursday { get; set; }
        public DayScheduleModel Friday { get; set; }

    }

    public class FacilityModel
    {
        public Guid FacilityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class DayScheduleModel
    {
        public WorkPeriodModel WorkPeriod { get; set; }
        public IEnumerable<FreeSlot> FreeSlots { get; set; } = new List<FreeSlot>();
    }

    public class WorkPeriodModel
    {
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int LunchStartHour { get; set; }
        public int LunchEndHour { get; set; }
    }

    public class FreeSlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
