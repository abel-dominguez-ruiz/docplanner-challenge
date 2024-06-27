namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public class TakeSlotResponse : CommandResult
    {

        private TakeSlotResponse(ResponseCode code)
        {
            Code = code;
        }

        private TakeSlotResponse(ResponseCode code, string errorMessage)
        {
            Code = code;
            ErrorMessage = errorMessage;
        }

        public ResponseCode Code { get; private set; }

        public static TakeSlotResponse Create()
            => new TakeSlotResponse(ResponseCode.Successful);

        public static TakeSlotResponse CreateWithError(string errorMessage)
            => new TakeSlotResponse(ResponseCode.Error, errorMessage);
    }

    public enum TakeSlotResponseCode
    {
        Successful,
        Error
    }
}
