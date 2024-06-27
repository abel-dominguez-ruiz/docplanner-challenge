namespace doctor_slot.Features.GetWeeklyAvailability.V1
{

    [ApiController]
    [ApiVersion("1.0")]
    [ControllerName("TakeSlot")]
    [Route("api/v{version:apiVersion}/TakeSlot")]
    public class TakeSlotController : MediatorControllerBase
    {
        public TakeSlotController(
        IMediator mediator,
        ILogger<Controller> logger)
        : base(mediator, logger)  { }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationError))]
        public async Task<IActionResult> Get([FromBody] TakeSlotModel model)
        {
            var request = new TakeSlotCommand(model);
            return await RequestAsync(request);
        }
    }
}
