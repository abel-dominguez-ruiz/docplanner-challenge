namespace Api.test.Feature.TakeSlot.V1
{
    public class TakeSlotMapperTests
    {
        [Fact]
        public void ToRequest_ShouldMapFieldsCorrectly()
        {
            // Arrange
            var model = new TakeSlotModel
            {
                Comments = "Some comments",
                End = "2024-06-25 09:30",
                Start = "2024-06-25 09:30",
                FacilityId = "Facility123",
                Patient = new PatientModel
                {
                    Email = "patient@example.com",
                    Name = "John",
                    Phone = "1234567890",
                    SecondName = "Doe"
                }
            };

            // Act
            var request = model.ToRequest();

            // Assert
            Assert.Equal(model.Comments, request.Comments);
            Assert.Equal(model.End, request.End);
            Assert.Equal(model.Start, request.Start);
            Assert.Equal(model.FacilityId, request.FacilityId);
            Assert.Equal(model.Patient.Email, request.Patient.Email);
            Assert.Equal(model.Patient.Name, request.Patient.Name);
            Assert.Equal(model.Patient.Phone, request.Patient.Phone);
            Assert.Equal(model.Patient.SecondName, request.Patient.SecondName);
        }
    }
}
