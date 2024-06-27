namespace VerticalSlice.Intrastructure.Exceptions
{
    public class RequestValidationException : Exception
    {
        public RequestValidationException(IEnumerable<ValidationError> errors)
            : base(errors != null ? string.Join("|", errors.Select((x, i) => $" Error Num.{i} ErrorMessage: {x.ErrorMessage}, AttemptedValue: {x.AttemptedValue}, PropertyName: {x.PropertyName}")) : string.Empty)
        {
            Errors = errors;
        }

        public IEnumerable<ValidationError> Errors { get; private set; } = new List<ValidationError>();
    }
}
