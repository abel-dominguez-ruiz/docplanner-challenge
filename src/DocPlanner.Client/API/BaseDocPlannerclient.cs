namespace DocPlanner.Client.API
{
    public abstract class BaseDocPlannerclient(
        HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<string> SendAsync(
            string requestUri,
            HttpMethod httpMethod,
            object content = null, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(httpMethod, requestUri);
            if (content is not null)
                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            using HttpContent responseContent = response.Content;
            string jsonString = await responseContent.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await HandleHttpErrors(response, $"Error trying to consume doc planner api with request {requestUri} at {DateTime.UtcNow} content: {jsonString}");
            }

            return jsonString;
        }

        private async Task HandleHttpErrors(HttpResponseMessage response, string message = null)
        {
            using HttpContent responseContent = response.Content;
            DocPlannerAPIClientException httpException = null;
            string contentString = await responseContent.ReadAsStringAsync();
            int statusCode = (int)response.StatusCode;

            if (400 <= statusCode && statusCode < 500 && string.IsNullOrWhiteSpace(contentString))
            {
                httpException = new DocPlannerAPIClientException("Unsuccessful response with no content", statusCode);
            }
            else
            {
                httpException = new DocPlannerAPIClientException(contentString, statusCode);
            }

            httpException.HttpStatusCode = statusCode;
            httpException.Headers = response.Headers.ToDictionary(a => a.Key, a => a.Value);

            throw httpException;
        }
    }
}
