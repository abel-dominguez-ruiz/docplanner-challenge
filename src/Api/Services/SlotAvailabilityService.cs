namespace doctor_slot.Services
{
    public class SlotAvailabilityService : ISlotAvailabilityService
    {
        private readonly ILogger<SlotAvailabilityService> _logger;
        private readonly IDocPlannerClient _docPlannerClient;
        public SlotAvailabilityService(ILogger<SlotAvailabilityService> logger,
            IDocPlannerClient docPlannerClient)
        {
            _logger = logger;
            _docPlannerClient = docPlannerClient;
        }

        public async Task<AvailabilityResponse> GetScheduleAsync(string dateFormat, CancellationToken cancellationToken)
        {
            _logger.LogInformation("START Executing {method} with date {date}", nameof(GetScheduleAsync), dateFormat);

            var result = await _docPlannerClient.GetWeeklyAvailabilityAsync<AvailabilityResponse>(dateFormat, cancellationToken);

            _logger.LogInformation("END Executing {method} with date {date}", nameof(GetScheduleAsync), dateFormat);

            return result;
        }

        public async Task<string> PostTakeSlotAsync(TakeSlotRequest slot, CancellationToken cancellationToken)
        {
            _logger.LogInformation("START Executing {method} with date {date}", nameof(PostTakeSlotAsync), slot.FacilityId);

            var result = await _docPlannerClient.PostTakeSlotAsync<string>(slot, cancellationToken);

            _logger.LogInformation("END Executing {method} with date {date}", nameof(PostTakeSlotAsync), slot.FacilityId);

            return result;
        }
    }
}
