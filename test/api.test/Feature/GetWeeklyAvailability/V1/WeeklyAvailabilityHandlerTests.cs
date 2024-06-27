using DocPlanner.Client.API;
using DocPlanner.Client.Models;

namespace Api.test.Feature.GetWeeklyAvailability.V1
{
    public class WeeklyAvailabilityHandlerTests
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
                },
                ""Wednesday"": {
                    ""WorkPeriod"": {
                        ""StartHour"": 9,
                        ""EndHour"": 17,
                        ""LunchStartHour"": 13,
                        ""LunchEndHour"": 14
                    },
                    ""BusySlots"": [
                        {
                            ""Start"": ""2024-06-19T09:30:00"",
                            ""End"": ""2024-06-19T09:40:00""
                        },
                        {
                            ""Start"": ""2024-06-19T09:00:00"",
                            ""End"": ""2024-06-19T09:10:00""
                        },
                        {
                            ""Start"": ""2024-06-19T09:10:00"",
                            ""End"": ""2024-06-19T09:20:00""
                        },
                        {
                            ""Start"": ""2024-06-19T09:20:00"",
                            ""End"": ""2024-06-19T09:30:00""
                        }
                    ]
                },
                ""Friday"": {
                    ""WorkPeriod"": {
                        ""StartHour"": 8,
                        ""EndHour"": 16,
                        ""LunchStartHour"": 13,
                        ""LunchEndHour"": 14
                    },
                    ""BusySlots"": [
                        {
                            ""Start"": ""2024-06-21T08:20:00"",
                            ""End"": ""2024-06-21T08:30:00""
                        },
                        {
                            ""Start"": ""2024-06-21T08:40:00"",
                            ""End"": ""2024-06-21T08:50:00""
                        },
                        {
                            ""Start"": ""2024-06-21T15:50:00"",
                            ""End"": ""2024-06-21T16:00:00""
                        },
                        {
                            ""Start"": ""2024-06-21T08:00:00"",
                            ""End"": ""2024-06-21T08:10:00""
                        },
                        {
                            ""Start"": ""2024-06-21T15:40:00"",
                            ""End"": ""2024-06-21T15:50:00""
                        },
                        {
                            ""Start"": ""2024-06-21T08:10:00"",
                            ""End"": ""2024-06-21T08:20:00""
                        },
                        {
                            ""Start"": ""2024-06-21T08:30:00"",
                            ""End"": ""2024-06-21T08:40:00""
                        }
                    ]
                }
            }";

        private readonly WeeklyAvailabilityModel model;
        private readonly Mock<ILogger<WeeklyAvailabilityHandler>> _logger;
        private readonly Mock<ISlotAvailabilityService> _mockSlotAvailabilityService;
        private readonly WeeklyAvailabilityHandler _sut;

        public WeeklyAvailabilityHandlerTests()
        {
            model = JsonConvert.DeserializeObject<WeeklyAvailabilityModel>(apiResponse);
            _mockSlotAvailabilityService = new Mock<ISlotAvailabilityService>();
            _logger = new Mock<ILogger<WeeklyAvailabilityHandler>>();
            _sut = new WeeklyAvailabilityHandler(_logger.Object, _mockSlotAvailabilityService.Object);
        }

        [Fact]
        public async Task When_Request_For_ValidMonday_Return_ValidResult()
        {
            // Arrange
            var testDate = "20240624";

            //Act
            SetupRepositoryOkRequest(JsonConvert.DeserializeObject<AvailabilityResponse>(apiResponse), testDate);
            var queryResult = await _sut.Handle(new WeeklyAvailabilityQuery(testDate), CancellationToken.None);

            // Assert
            Assert.True(queryResult.IsSuccess);
            Assert.NotNull(queryResult.Payload);
            _mockSlotAvailabilityService.VerifyAll();
        }


        [Fact]
        public async Task When_Request_For_ValidMonday_Return_BadRequest()
        {
            //Arrange
            var expectedErrorMessage = "Test Exception";

            //Act
            SetupRepositoryException(expectedErrorMessage);
            var queryResult = await _sut.Handle(new WeeklyAvailabilityQuery("2232323"), CancellationToken.None);

            // Assert
            Assert.False(queryResult.IsSuccess);
            Assert.Null(queryResult.Payload);
            Assert.NotNull(queryResult.ErrorMessage);
            Assert.Equal(expectedErrorMessage, queryResult.ErrorMessage);
             _mockSlotAvailabilityService.VerifyAll();
        }

        private void SetupRepositoryOkRequest(AvailabilityResponse result, string expectedDate)
        {
            _mockSlotAvailabilityService.Setup(x => x.GetScheduleAsync(It.Is<string>(item => item == expectedDate), CancellationToken.None))
                .ReturnsAsync(result).Verifiable();
        }

        private void SetupRepositoryException(string errorMessage)
        {
            _mockSlotAvailabilityService.Setup(x => x.GetScheduleAsync(It.IsAny<string>(), CancellationToken.None))
                .ThrowsAsync(new DocPlannerAPIClientException(errorMessage))
               .Verifiable();
        }
    }
}
