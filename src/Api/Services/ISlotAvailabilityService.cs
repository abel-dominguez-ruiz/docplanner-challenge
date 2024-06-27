namespace doctor_slot.Services
{
    public interface ISlotAvailabilityService
    {
        Task<AvailabilityResponse> GetScheduleAsync(string dateFormat, CancellationToken cancellationToken);
        Task<string> PostTakeSlotAsync(TakeSlotRequest slot, CancellationToken cancellationToken);
    }
}
