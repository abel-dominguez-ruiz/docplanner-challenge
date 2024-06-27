namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public class WeeklyAvailabilityQuery : IRequest<WeeklyAvailabilityResponse>
    {
        public WeeklyAvailabilityQuery(string datetime)
        {
            Date = datetime;

        }

        public string Date { get; }

        public DateTime FormatedDate
        {
            get
            {
                return DateTime.ParseExact(Date, "yyyyMMdd", null);
            }
        }
    }
}
