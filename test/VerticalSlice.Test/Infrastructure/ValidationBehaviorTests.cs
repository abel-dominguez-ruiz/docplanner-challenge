using FluentValidation.Results;
using FluentValidation;
using System;
using VerticalSlice.Intrastructure;
using VerticalSlice.Intrastructure.Exceptions;

namespace VerticalSlice.Test.Infrastructure
{
    public class ValidationBehaviorTests
    {
        [Fact]
        public async Task Handle_WithValidRequest_CallsNextHandler()
        {
            // Arrange
            var validators = new List<IValidator<TestRequest>>();
            var nextHandlerMock = new Mock<RequestHandlerDelegate<TestResponse>>();
            var request = new TestRequest();
            var response = new TestResponse();

            nextHandlerMock.Setup(nh => nh()).ReturnsAsync(response);

            var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);

            // Act
            var result = await behavior.Handle(request, nextHandlerMock.Object, CancellationToken.None);

            // Assert
            Assert.Equal(response, result);
            nextHandlerMock.Verify(nh => nh(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidRequest_ThrowsRequestValidationException()
        {
            // Arrange
            var validatorMock = new Mock<IValidator<TestRequest>>();
            var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("TestProperty", "Test error")
        });

            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);

            var validators = new List<IValidator<TestRequest>> { validatorMock.Object };
            var request = new TestRequest();

            var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<RequestValidationException>(() =>
                behavior.Handle(request, () => Task.FromResult(new TestResponse()), CancellationToken.None));

            Assert.Single(exception.Errors);
            Assert.Equal("TestProperty", exception.Errors.First().PropertyName);
            Assert.Equal("Test error", exception.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_WithNullNextHandler_ThrowsArgumentNullException()
        {
            // Arrange
            var validators = new List<IValidator<TestRequest>>();
            var request = new TestRequest();

            var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                behavior.Handle(request, null, CancellationToken.None));
        }

        [Fact]
        public async Task ValidateAsync_WithMultipleValidators_ReturnsCombinedErrors()
        {
            // Arrange
            var validator1 = new Mock<IValidator<TestRequest>>();
            var validationResult1 = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("TestProperty1", "Test error 1")
        });

            var validator2 = new Mock<IValidator<TestRequest>>();
            var validationResult2 = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("TestProperty2", "Test error 2")
        });

            validator1.Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(validationResult1);

            validator2.Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(validationResult2);

            var validators = new List<IValidator<TestRequest>> { validator1.Object, validator2.Object };
            var request = new TestRequest();

            var behavior = new TestValidationBehavior<TestRequest, TestResponse>(validators);

            // Act
            var errors = await behavior.ValidateAsync(request);

            // Assert
            Assert.Equal(2, errors.Count());
            Assert.Contains(errors, e => e.PropertyName == "TestProperty1" && e.ErrorMessage == "Test error 1");
            Assert.Contains(errors, e => e.PropertyName == "TestProperty2" && e.ErrorMessage == "Test error 2");
        }

        // test support classes
        public class TestRequest : IRequest<TestResponse>
        {
        }

        public class TestResponse
        {
        }

        public class TestValidationBehavior<TRequest, TResponse> : ValidationBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
        {
            public TestValidationBehavior(IEnumerable<IValidator<TRequest>> validators) : base(validators)
            {
            }

            public virtual async Task<IEnumerable<ValidationError>> ValidateAsync(TRequest request)
            {
                return await base.ValidateAsync(request);
            }
        }
    }
}
