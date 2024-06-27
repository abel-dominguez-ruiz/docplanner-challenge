namespace DocPlanner.Client.Test.Security
{
    public class BaseAuthenticationServiceTests
    {
        [Fact]
        public void GetAuthentication_ShouldReturnBasicAuthenticationHeader()
        {
            // Arrange
            string userName = "testUser";
            string userPassword = "testPassword";
            string expectedCredentials = $"{userName}:{userPassword}";
            byte[] byteCredentials = Encoding.UTF8.GetBytes(expectedCredentials);
            string base64Credentials = Convert.ToBase64String(byteCredentials);
            string expectedAuthenticationHeader = $"Basic {base64Credentials}";

            // Act
            var authService = new BaseAuthenticationService(userName, userPassword);
            string result = authService.GetAuthentication();

            // Assert
            Assert.Equal(expectedAuthenticationHeader, result);
        }

        [Theory]
        [InlineData("user1", "password1")]
        [InlineData("user2", "password2")]
        [InlineData("user3", "password3")]
        public void GetAuthentication_ShouldReturnCorrectBasicAuthenticationHeader(string userName, string userPassword)
        {
            // Arrange
            string expectedCredentials = $"{userName}:{userPassword}";
            byte[] byteCredentials = Encoding.UTF8.GetBytes(expectedCredentials);
            string base64Credentials = Convert.ToBase64String(byteCredentials);
            string expectedAuthenticationHeader = $"Basic {base64Credentials}";

            // Act
            var authService = new BaseAuthenticationService(userName, userPassword);
            string result = authService.GetAuthentication();

            // Assert
            Assert.Equal(expectedAuthenticationHeader, result);
        }
    }
}
