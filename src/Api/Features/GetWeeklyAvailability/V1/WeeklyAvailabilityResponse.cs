namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public class WeeklyAvailabilityResponse : QueryResult<WeeklyAvailabilityModel>
    {

        private WeeklyAvailabilityResponse(ResponseCode code, WeeklyAvailabilityModel model)
        {
            Code = code;
            Payload = model;
        }

        private WeeklyAvailabilityResponse(ResponseCode code, string errorMessage)
        {
            Code = code;
            ErrorMessage = errorMessage;
        }

        public ResponseCode Code { get; private set; }

        public static WeeklyAvailabilityResponse Create(WeeklyAvailabilityModel model)
            => new WeeklyAvailabilityResponse(ResponseCode.Successful, model);

        public static WeeklyAvailabilityResponse CreateWithError(string errorMessage)
            => new WeeklyAvailabilityResponse(ResponseCode.Error, errorMessage);
    }

    public enum ResponseCode
    {
        Successful,
        Error
    }
}
