namespace DocPlanner.Client.API
{
    public class DocPlannerAPIClientException : Exception
    {
        public DocPlannerAPIClientException(string message) : base(message) { }
        public DocPlannerAPIClientException(string message, int httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public int HttpStatusCode { get; set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; set; }
    }
}
