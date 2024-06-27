namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public class WeeklyAvailabilityHandler : IRequestHandler<WeeklyAvailabilityQuery, WeeklyAvailabilityResponse>
    {
        private readonly ILogger<WeeklyAvailabilityHandler> _logger;
        private readonly ISlotAvailabilityService _slotAvailabilityService;

        public WeeklyAvailabilityHandler(
            ILogger<WeeklyAvailabilityHandler> logger,
            ISlotAvailabilityService slotAvailabilityService)
        {
            ArgumentNullException.ThrowIfNull(logger);

            _logger = logger;
            _slotAvailabilityService = slotAvailabilityService;
        }

        public async Task<WeeklyAvailabilityResponse> Handle(WeeklyAvailabilityQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("START Executing {name} ", nameof(WeeklyAvailabilityHandler));
            WeeklyAvailabilityResponse result;

            try
            {
                var busySlots = await _slotAvailabilityService.GetScheduleAsync(request.Date,cancellationToken);
                var model = busySlots.ToModel(request.FormatedDate);
                result = WeeklyAvailabilityResponse.Create(model);
            }
            catch (Exception e)
            {
                result = WeeklyAvailabilityResponse.CreateWithError(e.Message);
            }

            _logger.LogInformation("END Executing {name} ", nameof(WeeklyAvailabilityHandler));

            return result;
        }
    }
}
