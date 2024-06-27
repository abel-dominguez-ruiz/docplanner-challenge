namespace VerticalSlice.Intrastructure.Command
{
    public interface IEmptyCommandResult
    {
        string ErrorMessage { get; }
        bool IsSuccess { get; }
    }


    public interface IQueryResult<T> : IEmptyCommandResult
    {
        T Payload { get; set; }
    }
}
