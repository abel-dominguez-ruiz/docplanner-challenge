namespace DocPlanner.Client.API
{
    public interface IDocPlannerClient
    {
        Task<TResult> GetWeeklyAvailabilityAsync<TResult>(string dateFormat, CancellationToken cancellationToken);
        Task<TResult> PostTakeSlotAsync<TResult>(TakeSlotRequest request, CancellationToken cancellationToken);
    }
}
