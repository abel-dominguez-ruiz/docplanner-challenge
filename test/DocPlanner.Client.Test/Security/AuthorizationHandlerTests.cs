using Moq.Protected;

namespace DocPlanner.Client.Test.Security
{
    public class AuthorizationHandlerTests
    {
        [Fact]
        public async Task SendAsync_ShouldAddAuthorizationHeader()
        {
            // Arrange
            var mockBaseAuthentication = new Mock<IBaseAuthentication>();
            mockBaseAuthentication.Setup(auth => auth.GetAuthentication()).Returns("Basic testToken");

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();

            var authorizationHandler = new AuthorizationHandler(mockBaseAuthentication.Object)
            {
                InnerHandler = handlerMock.Object
            };

            var httpClient = new HttpClient(authorizationHandler);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://test.com");

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Headers.Authorization.ToString() == "Basic testToken" &&
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("http://test.com")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task SendAsync_ShouldCallInnerHandler()
        {
            // Arrange
            var mockBaseAuthentication = new Mock<IBaseAuthentication>();
            mockBaseAuthentication.Setup(auth => auth.GetAuthentication()).Returns("Basic testToken");

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();

            var authorizationHandler = new AuthorizationHandler(mockBaseAuthentication.Object)
            {
                InnerHandler = handlerMock.Object
            };

            var httpClient = new HttpClient(authorizationHandler);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://test.com");

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
