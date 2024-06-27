namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public class WeeklyAvailabilityValidator : AbstractValidator<WeeklyAvailabilityQuery>
    {

        public WeeklyAvailabilityValidator()
        {
            RuleFor(x => x.Date).Must(BeAValidDate).WithMessage("DateFormat not valid");
            RuleFor(x => x.Date).Must(BeAValidMonday).WithMessage("DateFormat must be Monday");
        }


        private bool BeAValidDate(string dateTime)
        {
            return DateTime.TryParseExact(dateTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _);
        }

        private bool BeAValidMonday(string dateTime)
        {
            if (DateTime.TryParseExact(dateTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate.DayOfWeek == DayOfWeek.Monday;
            }
            return false;
        }
    }
}
