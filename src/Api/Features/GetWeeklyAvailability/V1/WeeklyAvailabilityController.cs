namespace doctor_slot.Features.GetWeeklyAvailability.V1
{

    [ApiController]
    [ApiVersion("1.0")]
    [ControllerName("WeeklyAvailability")]
    [Route("api/v{version:apiVersion}/WeeklyAvailability")]
    public class WeeklyAvailabilityController : MediatorControllerBase
    {
        public WeeklyAvailabilityController(
        IMediator mediator,
        ILogger<Controller> logger)
        : base(mediator, logger)  { }


        [HttpGet("{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationError))]
        public async Task<IActionResult> Get([FromRoute] string date)
        {
            var request = new WeeklyAvailabilityQuery(date);
            return await RequestAsync(request);
        }
    }
}
