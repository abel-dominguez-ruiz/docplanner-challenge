namespace VerticalSlice.Intrastructure.Exceptions
{
    public static class LoggerExceptionExtensions
    {
        public static void LogException(this ILogger logger, Exception ex)
        {
            logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (ex == null)
            {
                return;
            }

            if (ex is RequestValidationException requestValidation)
            {
                var validationErrors = string.Join("|", requestValidation.Errors.Select((x, i) => $" Error Num.{i} ErrorMessage: {x.ErrorMessage}, AttemptedValue: {x.AttemptedValue}, PropertyName: {x.PropertyName}"));

                logger.LogError(ex, $"Validation exception: Message: {requestValidation.Message}. Validation Errors: {validationErrors}");
            }
            else
            {
                logger.LogError(ex, "Handled exception: Message:" + ex.Message);
            }
        }
    }
}
