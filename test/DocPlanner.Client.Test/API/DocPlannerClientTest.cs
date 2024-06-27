using Moq.Protected;

namespace DocPlanner.Client.Test.API
{
    public class DocPlannerClientTest
    {

        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly DocPlannerClient _docPlannerClient;

        public DocPlannerClientTest()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com")  // Establece la BaseAddress para HttpClient
            };
            _docPlannerClient = new DocPlannerClient(_httpClient);
        }

        [Fact]
        public async Task GetWeeklyAvailabilityAsync_ShouldReturnDeserializedResult()
        {
            // Arrange
            var dateFormat = "2024-06-26";
            var expectedResult = new { Availability = "Available" };
            var jsonResponse = JsonConvert.SerializeObject(expectedResult);

            SetupHttpMessageHandlerMock(HttpStatusCode.OK, jsonResponse);

            // Act
            var result = await _docPlannerClient.GetWeeklyAvailabilityAsync<dynamic>(dateFormat, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult.Availability.ToString(), result.Availability.ToString());
            VerifyHttpRequest(HttpMethod.Get, new Uri(_httpClient.BaseAddress, $"/api/availability/getWeeklyAvailability/{dateFormat}").ToString());
        }

        [Fact]
        public async Task PostTakeSlotAsync_ShouldReturnDeserializedResult()
        {
            // Arrange
            var request = new TakeSlotRequest { /* Populate with necessary properties */ };
            var expectedResult = new { Success = true };
            var jsonResponse = JsonConvert.SerializeObject(expectedResult);

            SetupHttpMessageHandlerMock(HttpStatusCode.OK, jsonResponse);

            // Act
            var result = await _docPlannerClient.PostTakeSlotAsync<dynamic>(request, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult.Success.ToString(), result.Success.ToString());
            VerifyHttpRequest(HttpMethod.Post, new Uri(_httpClient.BaseAddress, "/api/availability/takeslot").ToString());
        }

        [Fact]
        public async Task SendAsync_ShouldThrowException_OnErrorResponse()
        {
            // Arrange
            var requestUri = "/api/error";
            var errorMessage = "Unsuccessful response with no content";
            SetupHttpMessageHandlerMock(HttpStatusCode.BadRequest, string.Empty);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DocPlannerAPIClientException>(() => _docPlannerClient.SendAsync(requestUri, HttpMethod.Get));
            Assert.Equal(errorMessage, exception.Message);
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.HttpStatusCode);
        }

        private void SetupHttpMessageHandlerMock(HttpStatusCode statusCode, string content)
        {
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                });
        }

        private void VerifyHttpRequest(HttpMethod method, string requestUri)
        {
            _httpMessageHandlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == method && req.RequestUri.ToString() == requestUri),
                    ItExpr.IsAny<CancellationToken>());
        }
    }
}
