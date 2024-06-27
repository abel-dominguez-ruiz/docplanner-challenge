namespace VerticalSlice.Intrastructure
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            return OnHandleAsync(request, next);
        }

        protected virtual async Task<TResponse> OnHandleAsync(TRequest request, RequestHandlerDelegate<TResponse> nextHandler)
        {
            nextHandler = nextHandler ?? throw new ArgumentNullException(nameof(nextHandler));

            var errors = await ValidateAsync(request);
            if (errors.Any())
                throw new RequestValidationException(errors);

            return await nextHandler();
        }

        protected virtual async Task<IEnumerable<ValidationError>> ValidateAsync(TRequest request)
        {
            var result = new List<ValidationError>();
            var context = new ValidationContext<TRequest>(request);
            foreach (var validator in _validators)
            {
                var r = await validator.ValidateAsync(context);
                result.AddRange(r.Errors.Select(failure => new ValidationError
                {
                    PropertyName = failure.PropertyName,
                    ErrorMessage = failure.ErrorMessage,
                    AttemptedValue = failure.AttemptedValue
                }));

            }

            return result;
        }
    }

}
