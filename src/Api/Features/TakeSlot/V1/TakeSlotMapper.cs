namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public static class TakeslotMapper
    {
        public static TakeSlotRequest ToRequest(this TakeSlotModel model)
        {
            return new TakeSlotRequest()
            {
                Comments = model.Comments,
                End = model.End,
                Start = model.Start,
                FacilityId = model.FacilityId,
                Patient = new Patient() { 
                
                    Email = model.Patient.Email,
                    Name = model.Patient.Name,
                    Phone = model.Patient.Phone,
                    SecondName = model.Patient.SecondName
                }
            };
        }
    }
}
