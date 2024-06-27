namespace DocPlanner.Client.Models
{
    public class TakeSlotRequest
    {
        public string FacilityId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Comments { get; set; }
        public Patient Patient { get; set; }
    }

    public class Patient
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
