namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public class TakeSlotValidator : AbstractValidator<TakeSlotCommand>
    {

        public TakeSlotValidator()
        {
            RuleFor(x => x.Model.FacilityId).NotNull().WithMessage("'{PropertyName}' Id is required")
                .NotEmpty().WithMessage("'{PropertyName}' Id cannot be empty");

            RuleFor(x => x.Model.Start).Must(BeAValidDate).WithMessage("'{PropertyName}' must be Valid date with format yyyy-MM-dd HH:mm");
            RuleFor(x => x.Model.End).Must(BeAValidDate).WithMessage("'{PropertyName}' must be Valid date with format yyyy-MM-dd HH:mm");

            RuleFor(x => x.Model)
          .Must(model => BeEndDateAfterStartDate(model.Start, model.End))
          .WithMessage("'End' date must be after 'Start' date");


            RuleFor(x => x.Model.Patient.Name).NotNull().WithMessage("'{PropertyName}' is required");
            RuleFor(x => x.Model.Patient.SecondName).NotNull().WithMessage("'{PropertyName}' is required");
            RuleFor(x => x.Model.Patient.Email).NotNull().WithMessage("'{PropertyName}' is required");
            RuleFor(x => x.Model.Patient.Phone).NotNull().WithMessage("'{PropertyName}' is required");
        }

        private bool BeAValidDate(string dateTime)
        {
            return DateTime.TryParseExact(dateTime, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out _);
        }


        private bool BeEndDateAfterStartDate(string start, string end)
        {
            if (DateTime.TryParseExact(start, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime startDate) &&
                DateTime.TryParseExact(end, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
            {
                return endDate > startDate;
            }
            return false;
        }
    }
}
