namespace Api.test.Feature.TakeSlot.V1
{
    public class TakeSlotHandlerValidatorTests
    {
        private readonly TakeSlotValidator _sut;

        public TakeSlotHandlerValidatorTests()
        {
            _sut = new TakeSlotValidator();
        }

        [Theory]        
        [InlineData("test_id", "2024-06-26 9:30", "2024-06-26 9:20", false, 3)]
        [InlineData("test_id", "2024-06-26 09:00", "2024-06-26 09:10", true, 0)]
        [InlineData("", "2024-06-26 09:00", "2024-06-26 09:10", false, 1)]
        public void Validate_TakeSlotWithValidPatient_Then_ReturnsExpected(
           string facilityid,
           string startDate,
           string endDate,
           bool expected,
           int errorCount)
        {

            var model = new TakeSlotModel()
            {
                Patient = new PatientModel()
                {
                    Email = "emailTEst",
                    Name = "test name",
                    Phone = "88232332",
                    SecondName = "23232333"
                },
                Comments = "",
                End = endDate,
                Start = startDate,
                FacilityId = facilityid
            };

            var request = new TakeSlotCommand(model);
            var actual = _sut.Validate(request);

            actual.IsValid.Should().Be(expected);
            actual.Errors.Should().HaveCount(errorCount);
        }


        [Theory]
        [InlineData("name", "name2", "611343421", "email@gmail.com", true, 0)]
        [InlineData(null, "name2", "611343421", "email@gmail.com", false, 1)]
        [InlineData("name", null, "611343421", "email@gmail.com", false, 1)]
        [InlineData("name", "name2", null, "email@gmail.com", false, 1)]
        [InlineData("name", "name2", "611343421", null, false, 1)]
        [InlineData("", "", "", "", true, 0)]
        public void Validate_TakeSlot_PatientInfo_Then_ReturnsExpected(
        string name,
        string secondName,
        string phone,
        string email,
        bool expected,
        int errorCount)
        {
            var model = new TakeSlotModel()
            {
                Patient = new PatientModel()
                {
                    Name = name,
                    SecondName = secondName,
                    Phone = phone,
                    Email = email
                },
                Comments = "test",
                End = "2024-06-26 09:40",
                Start = "2024-06-26 09:30",
                FacilityId = "test"
            };

            var request = new TakeSlotCommand(model);
            var actual = _sut.Validate(request);

            actual.IsValid.Should().Be(expected);
            actual.Errors.Should().HaveCount(errorCount);
        }

    }
}
