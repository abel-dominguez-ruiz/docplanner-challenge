namespace VerticalSlice.Intrastructure.Exceptions
{
    public class ValidationError
    {
        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }

        public object AttemptedValue { get; set; }
    }
}
