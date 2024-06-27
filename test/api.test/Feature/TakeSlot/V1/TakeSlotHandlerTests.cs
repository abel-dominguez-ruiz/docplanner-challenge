namespace Api.test.Feature.TakeSlot.V1
{
    public class TakeSlotHandlerTests
    {
        private TakeSlotModel model;
        private readonly Mock<ILogger<TakeSlotHandler>> _logger;
        private readonly Mock<ISlotAvailabilityService> _mockSlotAvailabilityService;
        private readonly TakeSlotHandler _sut;

        public TakeSlotHandlerTests()
        {
            _mockSlotAvailabilityService = new Mock<ISlotAvailabilityService>();
            _logger = new Mock<ILogger<TakeSlotHandler>>();
            _sut = new TakeSlotHandler(_logger.Object, _mockSlotAvailabilityService.Object);
        }


        [Fact]
        public async Task When_Valid_TakeSlot()
        {
            model = new TakeSlotModel()
            {
                Comments = "test",
                End = "test",
                FacilityId = "test",
                Start = "test",
                Patient = new PatientModel()
                {
                    Email = "test",
                    Name = "test",
                    Phone = "test",
                    SecondName = "test"
                }
            };
            var command = new TakeSlotCommand(model);

            SetupRepositoryOkRequest();
            var handleResult = await _sut.Handle(new TakeSlotCommand(model), CancellationToken.None);

            Assert.True(handleResult.IsSuccess);
            _mockSlotAvailabilityService.Verify(client =>
                          client.PostTakeSlotAsync(It.IsAny<TakeSlotRequest>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task When_Error_Take_Slot()
        {
            //Arrange
            model = new TakeSlotModel()
            {
                Comments = "test",
                End = "test",
                FacilityId = "test",
                Start = "test",
                Patient = new PatientModel()
                {
                    Email = "test",
                    Name = "test",
                    Phone = "test",
                    SecondName = "test"
                }
            };

            var exceptionMessage = "Test Exception";
            //Act
            SetupRepositoryException(exceptionMessage);
            var handleResult = await _sut.Handle(new TakeSlotCommand(model), CancellationToken.None);


            //Assert
            Assert.False(handleResult.IsSuccess);
            Assert.Equal(exceptionMessage, handleResult.ErrorMessage);
        }

        private void SetupRepositoryOkRequest()
        {
            _mockSlotAvailabilityService.Setup(x => x.PostTakeSlotAsync(It.IsAny<TakeSlotRequest>(), CancellationToken.None))
                .ReturnsAsync("").Verifiable();
        }

        private void SetupRepositoryException(string errorMessage)
        {
            _mockSlotAvailabilityService.Setup(x => x.PostTakeSlotAsync(It.IsAny<TakeSlotRequest>(), CancellationToken.None))
                .ThrowsAsync(new DocPlannerAPIClientException(errorMessage))
               .Verifiable();
        }
    }
}
