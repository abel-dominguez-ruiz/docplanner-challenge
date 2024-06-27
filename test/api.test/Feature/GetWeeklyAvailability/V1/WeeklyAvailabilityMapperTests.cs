
using DocPlanner.Client.Models;
using System.Reflection.PortableExecutable;

namespace Api.test.Feature.GetWeeklyAvailability.V1
{
    public class WeeklyAvailabilityMapperTests
    {
        string apiResponse = @"
            {
                ""Facility"": {
                    ""FacilityId"": ""ff506e69-3d8c-4be5-9ee9-8e712b12ef5b"",
                    ""Name"": ""Las Palmeras"",
                    ""Address"": ""Plaza de la independencia 36, 38006 Santa Cruz de Tenerife""
                },
                ""SlotDurationMinutes"": 10,
                ""Monday"": {
                    ""WorkPeriod"": {
                        ""StartHour"": 9,
                        ""EndHour"": 17,
                        ""LunchStartHour"": 13,
                        ""LunchEndHour"": 14
                    },
                    ""BusySlots"": [
                        {
                            ""Start"": ""2024-06-17T09:30:00"",
                            ""End"": ""2024-06-17T09:40:00""
                        },
                        {
                            ""Start"": ""2024-06-17T09:20:00"",
                            ""End"": ""2024-06-17T09:30:00""
                        },
                        {
                            ""Start"": ""2024-06-17T09:00:00"",
                            ""End"": ""2024-06-17T09:10:00""
                        },
                        {
                            ""Start"": ""2024-06-17T09:00:00"",
                            ""End"": ""2024-06-17T09:10:00""
                        }
                    ]
                } ,
                ""Friday"": {
                    ""WorkPeriod"":null,
                    ""BusySlots"":null
                }             
            }";


        private readonly AvailabilityResponse _availabilityResponse;
        public WeeklyAvailabilityMapperTests()
        {
            _availabilityResponse = JsonConvert.DeserializeObject<AvailabilityResponse>(apiResponse);
        }

        [Fact]
        public void Valid_Map_Facility_ToModel()
        {
            // Arrange
            var facilityExpected = new Facility()
            {
                Address = "test addres",
                FacilityId = Guid.NewGuid(),
                Name = "TestnName"
            };

            //Act
            var result = facilityExpected.ToModel();

            //Assert
            Assert.Equal(result.Address, facilityExpected.Address);
            Assert.Equal(result.Name, facilityExpected.Name);
            Assert.Equal(result.FacilityId, facilityExpected.FacilityId);
        }

        [Fact]
        public void Valid_Map_DaySchedule_ToModel_WhenNoSBusySlot_Exist()
        {
            // Arrange
            var datetime = new DateTime(2024, 6, 24);
            int slotDuration = 30;
            var inputSub = new DaySchedule()
            {
                WorkPeriod = new WorkPeriod()
                {
                    StartHour = 9,
                    LunchStartHour = 10,
                    LunchEndHour = 11,
                    EndHour = 12
                },
                BusySlots = new List<BusySlot>()
            };


            //Act
            var result = inputSub.ToModel(slotDuration, datetime);

            //Assert
            Assert.Equal(4, result.FreeSlots.Count());
        }

        [Fact]
        public void Valid_Map_DaySchedule_ToModel_WhenSBusySlot_Exist()
        {
            // Arrange
            var datetime = new DateTime(2024, 6, 24);
            var busyStartDate = datetime.AddHours(9);
            var busyEndDate = datetime.AddHours(9).AddMinutes(30);

            int slotDuration = 30;
            var inputSub = new DaySchedule()
            {
                WorkPeriod = new WorkPeriod()
                {
                    StartHour = 9,
                    LunchStartHour = 10,
                    LunchEndHour = 11,
                    EndHour = 12
                },
                BusySlots = new List<BusySlot>()
                { new BusySlot()
                    {
                            Start = busyStartDate,
                            End = busyEndDate
                    }
                }
            };

            //Act
            var result = inputSub.ToModel(slotDuration, datetime);

            //Assert
            Assert.Equal(3, result.FreeSlots.Count());
            Assert.DoesNotContain(result.FreeSlots, x => x.Start == busyStartDate);
        }

        [Fact]
        public void No_Slot_When_SlotDuration_Is_Higher_Than_Workshour()
        {
            //Act
            int slotDuration = 100;
            var inputSub = new DaySchedule()
            {
                WorkPeriod = new WorkPeriod()
                {
                    StartHour = 9,
                    LunchStartHour = 10,
                    LunchEndHour = 11,
                    EndHour = 12
                },
                BusySlots = new List<BusySlot>()
            };

            //Arrange
            DateTime datetime = new DateTime(2024, 6, 24);
            var result = inputSub.ToModel(slotDuration, datetime);

            //Assert
            Assert.Empty(result.FreeSlots);
        }

        [Fact]
        public void NullDaySchedule_Return_EmptySchedule()
        {
            //Act
            var datetime = new DateTime(2024, 6, 24);

            //Arrange
            var result = _availabilityResponse.ToModel(datetime);

            //Assert
            Assert.NotNull(result.Monday);
            Assert.Null(result.Tuesday);
            Assert.Null(result.Wednesday);
            Assert.Null(result.Thursday);
            Assert.NotNull(result.Friday);
            Assert.Empty(result.Friday.FreeSlots);
            Assert.Null(result.Friday.WorkPeriod);
        }
    }
}
