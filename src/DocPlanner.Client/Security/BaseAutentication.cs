namespace DocPlanner.Client.Security
{
    public class BaseAuthenticationService : IBaseAuthentication
    {
        private readonly string BasicAuthentication;
        public BaseAuthenticationService(
            string userName,
            string userPassword)
        {
            string credentials = $"{userName}:{userPassword}";
            byte[] byteCredentials = Encoding.UTF8.GetBytes(credentials);
            string base64Credentials = Convert.ToBase64String(byteCredentials);
            BasicAuthentication = $"Basic {base64Credentials}";
        }

        public string GetAuthentication()
        {
            return BasicAuthentication;
        }
    }
}
