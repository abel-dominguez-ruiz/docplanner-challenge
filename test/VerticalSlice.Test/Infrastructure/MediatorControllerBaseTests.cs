using VerticalSlice.Intrastructure.Exceptions;

namespace VerticalSlice.Test.Infrastructure
{
    public class MediatorControllerBaseTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly TestMediatorController _controller;

        public MediatorControllerBaseTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger>();
            _controller = new TestMediatorController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task RequestAsync_NullRequest_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.RequestAsync<string>(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task RequestAsync_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new Mock<IRequest<string>>().Object;
            _mediatorMock.Setup(m => m.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync("response");

            // Act
            var result = await _controller.RequestAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("response", okResult.Value);
        }

        [Fact]
        public async Task RequestAsync_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var request = new Mock<IRequest<string>>().Object;
            var exception = new Exception("Test exception");
            _mediatorMock.Setup(m => m.Send(request, It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            // Act
            var result = await _controller.RequestAsync(request);

            // Assert
            var objectResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task HandleExceptionAsync_HandlesRequestValidationException_ReturnsBadRequest()
        {
            // Arrange
            var exception = new RequestValidationException(
                new List<ValidationError>{ new ValidationError(){
                ErrorMessage = "Validation Error"
                } });

            // Act
            var result = await _controller.HandleExceptionAsync(exception);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exception.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task HandleExceptionAsync_HandlesBusinessValidationException_ReturnsConflict()
        {
            // Arrange
            var exception = new BusinessValidationException("Business validation error");

            // Act
            var result = await _controller.HandleExceptionAsync(exception);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(exception.Message, conflictResult.Value);
        }

        [Fact]
        public async Task HandleResponseAsync_NullResponse_ReturnsNotFound()
        {
            // Act
            var result = await _controller.HandleResponseAsync<string>(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task HandleResponseAsync_SuccessfulQueryResult_ReturnsOk()
        {
            // Arrange
            var response = new TestQueryResult<string>("Payload", "");

            // Act
            var result = await _controller.HandleResponseAsync(response);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var payload = Assert.IsType<TestQueryResult<string>>(okResult.Value);
            Assert.Equal("Payload", ((TestQueryResult<string>)okResult.Value).Payload);
        }

        [Fact]
        public async Task HandleResponseAsync_UnsuccessfulEmptyCommandResult_ReturnsBadRequest()
        {
            // Arrange
            var response = new TestEmptyCommandResult("Error");

            // Act
            var result = await _controller.HandleResponseAsync(response);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // Controlador de prueba que hereda de MediatorControllerBase para probar los métodos protegidos
        public class TestMediatorController(IMediator mediator, ILogger logger) : MediatorControllerBase(mediator, logger)
        {
            public new Task<IActionResult> RequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) where TResponse : class
                => base.RequestAsync(request, cancellationToken);

            public Task<IActionResult> InvokeHandleResponseAsync<TResponse>(TResponse response) where TResponse : class
            {
                return HandleResponseAsync(response);
            }
            public new Task<IActionResult> HandleResponseAsync<TResponse>(TResponse response) where TResponse : class => base.HandleResponseAsync(response);

            public new Task<IActionResult> HandleExceptionAsync(Exception ex) => base.HandleExceptionAsync(ex);
        }

        // Clases de soporte para las pruebas
        public class TestQueryResult<TResponse> : QueryResult<TResponse>
        {
            public TestQueryResult(TResponse payload, string errorMessage)
            {
                Payload = payload;
                ErrorMessage = errorMessage;
            }
        }

        public class TestEmptyCommandResult : CommandResult
        {
            public TestEmptyCommandResult(string errorMessage)
            {
                ErrorMessage = errorMessage;
            }
        }
    }
}