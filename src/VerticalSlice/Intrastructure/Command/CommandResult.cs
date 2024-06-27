namespace VerticalSlice.Intrastructure.Command
{
    public abstract class CommandResult : IEmptyCommandResult
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

        public CommandResult WithErrors(params string[] errors)
        {
            ErrorMessage = string.Join(",", errors);
            return this;
        }
    }

    public abstract class QueryResult<T> : CommandResult, IQueryResult<T>
    {
        public T Payload { get; set; }

        public CommandResult WithPayload(T payload)
        {
            Payload = payload;
            ErrorMessage = string.Empty;
            return this;
        }
    }
}
