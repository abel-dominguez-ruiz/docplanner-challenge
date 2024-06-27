namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public static class WeeklyAvailabilityMapper
    {
        public static WeeklyAvailabilityModel ToModel(this AvailabilityResponse availabilityResponse, DateTime mondayDate)
        {
            return new WeeklyAvailabilityModel()
            {
                Facility = availabilityResponse.Facility.ToModel(),
                SlotDurationMinutes = availabilityResponse.SlotDurationMinutes,
                Monday = availabilityResponse.Monday?.ToModel(availabilityResponse.SlotDurationMinutes, mondayDate),
                Tuesday = availabilityResponse.Tuesday?.ToModel(availabilityResponse.SlotDurationMinutes, mondayDate.AddDays(1)),
                Wednesday = availabilityResponse.Wednesday?.ToModel(availabilityResponse.SlotDurationMinutes, mondayDate.AddDays(2)),
                Thursday = availabilityResponse.Thrusday?.ToModel(availabilityResponse.SlotDurationMinutes, mondayDate.AddDays(3)),
                Friday = availabilityResponse.Friday?.ToModel(availabilityResponse.SlotDurationMinutes, mondayDate.AddDays(4))
            };
        }

        public static FacilityModel ToModel(this Facility facility)
        {
            return new FacilityModel()
            {
                FacilityId = facility.FacilityId,
                Address = facility.Address,
                Name = facility.Name
            };
        }

        public static WorkPeriodModel ToModel(this WorkPeriod workPeriod)
        {
            return new WorkPeriodModel()
            {
                StartHour = workPeriod.StartHour,
                EndHour = workPeriod.EndHour,
                LunchStartHour = workPeriod.LunchStartHour,
                LunchEndHour = workPeriod.LunchEndHour
            };
        }

        public static DayScheduleModel ToModel(this DaySchedule daySchedule, int slotduration, DateTime dateTime)
        {
            if (daySchedule.WorkPeriod is null)
                return new DayScheduleModel();

            var preLunchSlots = GenerateFreeSlots(dateTime, daySchedule.WorkPeriod.StartHour, daySchedule.WorkPeriod.LunchStartHour, slotduration, daySchedule.BusySlots);
            var postLunchSlot = GenerateFreeSlots(dateTime, daySchedule.WorkPeriod.LunchEndHour, daySchedule.WorkPeriod.EndHour, slotduration, daySchedule.BusySlots);
            var allFreeSots = preLunchSlots.Union(postLunchSlot).ToList();

            return new DayScheduleModel()
            {
                WorkPeriod = daySchedule.WorkPeriod.ToModel(),
                FreeSlots = allFreeSots
            };
        }

        private static IEnumerable<FreeSlot> GenerateFreeSlots(DateTime dateTime, int from, int to, int slotduration, IEnumerable<BusySlot> busySlots)
        {
            var freeSlots = new List<FreeSlot>();

            var startHour = dateTime.Date.AddHours(from);
            var endHour = dateTime.Date.AddHours(to);

            while (startHour < endHour)
            {
                var endSlot = startHour.AddMinutes(slotduration);
                if (endSlot > endHour)
                    break;

                if (!busySlots.Any(x => x.Start == startHour))
                {
                    freeSlots.Add(new FreeSlot
                    {
                        Start = startHour,
                        End = endSlot
                    });
                }

                startHour = endSlot;
            }

            return freeSlots;
        }

    }
}
