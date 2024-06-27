namespace VerticalSlice
{
    public abstract class MediatorControllerBase : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public MediatorControllerBase(
            IMediator mediator,
            ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected ILogger Logger => _logger;

        protected virtual async Task<IActionResult> RequestAsync<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default) where TResponse : class
        {
            if (request == null) return BadRequest();

            try
            {
                var response = await _mediator.Send(request, cancellationToken);
                return await HandleResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        protected virtual Task<IActionResult> HandleExceptionAsync(Exception ex)
        {
            ex = ex ?? throw new ArgumentNullException(nameof(ex));
            _logger.LogException(ex);

            return Task.FromResult<IActionResult>(ex switch
            {
                RequestValidationException rex => BadRequest(rex.Errors),
                BusinessValidationException bex => Conflict(bex.Message),
                _ => StatusCode((int)HttpStatusCode.InternalServerError)
            }); ;
        }

        protected virtual Task<IActionResult> HandleResponseAsync<TResponse>(TResponse response) where TResponse : class
        {
            return Task.FromResult<IActionResult>(response switch
            {
                null => NotFound(),
                IQueryResult<TResponse> r when r.IsSuccess => Ok(r.Payload),
                IEmptyCommandResult r when !r.IsSuccess => BadRequest(response),
                _ => Ok(response)
            });
        }
    }
}
