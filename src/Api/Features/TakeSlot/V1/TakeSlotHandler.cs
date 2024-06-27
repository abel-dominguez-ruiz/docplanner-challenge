namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public class TakeSlotHandler : IRequestHandler<TakeSlotCommand, TakeSlotResponse>
    {
        private readonly ILogger<TakeSlotHandler> _logger;
        private readonly ISlotAvailabilityService _slotAvailabilityService;

        public TakeSlotHandler(
            ILogger<TakeSlotHandler> logger,
            ISlotAvailabilityService slotAvailabilityService)
        {
            ArgumentNullException.ThrowIfNull(logger);

            _logger = logger;
            _slotAvailabilityService = slotAvailabilityService;
        }

        public async Task<TakeSlotResponse> Handle(TakeSlotCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("START Executing {name} ", nameof(TakeSlotHandler));
            TakeSlotResponse result;

            try
            {
                await _slotAvailabilityService.PostTakeSlotAsync(request.Model.ToRequest(), cancellationToken);
                result = TakeSlotResponse.Create();
            }
            catch (Exception e)
            {
                result = TakeSlotResponse.CreateWithError(e.Message);
            }

            _logger.LogInformation("END Executing {name} ", nameof(TakeSlotHandler));

            return result;
        }
    }
}
