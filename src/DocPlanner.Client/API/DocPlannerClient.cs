namespace DocPlanner.Client.API
{
    public class DocPlannerClient : BaseDocPlannerclient, IDocPlannerClient
    {
        public DocPlannerClient(HttpClient httpClient) : base(httpClient)
        {
        }

        private const string GET_AVAILABILITY = "/api/availability/getWeeklyAvailability";

        public async Task<TResult> GetWeeklyAvailabilityAsync<TResult>(string dateFormat, CancellationToken cancellationToken)
        {
            var result = await SendAsync($"{GET_AVAILABILITY}/{dateFormat}", HttpMethod.Get, cancellationToken: cancellationToken);
            return JsonConvert.DeserializeObject<TResult>(result);
        }


        private const string TAKE_SLOT = "/api/availability/takeslot";

        public async Task<TResult> PostTakeSlotAsync<TResult>(TakeSlotRequest request, CancellationToken cancellationToken)
        {
            var result = await SendAsync(TAKE_SLOT, HttpMethod.Post, request, cancellationToken: cancellationToken);
            return JsonConvert.DeserializeObject<TResult>(result);
        }
    }
}
