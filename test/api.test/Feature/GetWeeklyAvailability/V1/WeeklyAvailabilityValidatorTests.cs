namespace Api.test.Feature.GetWeeklyAvailability.V1
{
    public class WeeklyAvailabilityValidatorTests
    {
        private readonly WeeklyAvailabilityValidator _sut;

        public WeeklyAvailabilityValidatorTests()
        {
            _sut = new WeeklyAvailabilityValidator();
        }

        [Theory]
        [InlineData("notdate", false, 2)]
        [InlineData("20240624", true, 0)]
        [InlineData("20240625", false, 1)]
        public void Validate_DateInputRequest_Then_ReturnsExpected(
           string date,
           bool expected,
           int errorCount)
        {
            var request = new WeeklyAvailabilityQuery(date);

            var actual = _sut.Validate(request);

            actual.IsValid.Should().Be(expected);
            actual.Errors.Should().HaveCount(errorCount);
        }
    }
}
