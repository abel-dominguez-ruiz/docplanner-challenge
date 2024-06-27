using DocPlanner.Client.API;
using DocPlanner.Client.Models;

namespace Api.test.Services
{
    public class SlotAvailabilityServiceTests
    {
        private readonly Mock<ILogger<SlotAvailabilityService>> _mockLogger;
        private readonly Mock<IDocPlannerClient> _mockDocPlannerClient;
        private readonly SlotAvailabilityService _service;

        public SlotAvailabilityServiceTests()
        {
            _mockLogger = new Mock<ILogger<SlotAvailabilityService>>();
            _mockDocPlannerClient = new Mock<IDocPlannerClient>();
            _service = new SlotAvailabilityService(_mockLogger.Object, _mockDocPlannerClient.Object);
        }

        [Fact]
        public async Task GetScheduleAsync_ShouldLogAndCallClient()
        {
            // Arrange
            var dateFormat = "2024-06-26";
            var cancellationToken = new CancellationToken();
            var expectedResponse = new AvailabilityResponse();

            _mockDocPlannerClient
                .Setup(client => client.GetWeeklyAvailabilityAsync<AvailabilityResponse>(dateFormat, cancellationToken))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.GetScheduleAsync(dateFormat, cancellationToken);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockDocPlannerClient.Verify(client =>
                client.GetWeeklyAvailabilityAsync<AvailabilityResponse>(dateFormat, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task PostTakeSlotAsync_ShouldLogAndCallClient()
        {
            // Arrange
            var takeSlotRequest = new TakeSlotRequest { FacilityId = "facility_123" };
            var cancellationToken = new CancellationToken();
            var expectedResponse = "Success";

            _mockDocPlannerClient
                .Setup(client => client.PostTakeSlotAsync<string>(takeSlotRequest, cancellationToken))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.PostTakeSlotAsync(takeSlotRequest, cancellationToken);

            // Assert
            Assert.Equal(expectedResponse, result);

            _mockDocPlannerClient.Verify(client =>
                client.PostTakeSlotAsync<string>(takeSlotRequest, cancellationToken), Times.Once);
        }
    }
}
