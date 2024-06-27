namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public class TakeSlotModel
    {
        public string FacilityId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Comments { get; set; }
        public PatientModel Patient { get; set; }
    }

    public class PatientModel
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
