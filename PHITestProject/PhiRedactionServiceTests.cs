using Microsoft.Extensions.Logging;
using Moq;
using PHIRedationApplication.Server.Services;

namespace PHITestProject
{
    public class PhiRedactionServiceTests
    {
        private readonly Mock<ILogger<PhiRedactionService>> _mockLogger;
        private readonly PhiRedactionService _phiRedactionService;

        public PhiRedactionServiceTests()
        {
            _mockLogger = new Mock<ILogger<PhiRedactionService>>();
            _phiRedactionService = new PhiRedactionService(_mockLogger.Object);
        }

        [Theory]
        [InlineData("Name: John Doe", "Name: [REDACTED]")]
        [InlineData("Patient Name: Jane Smith", "Patient Name: [REDACTED]")]
        [InlineData("Date of Birth: 01/01/2000", "Date of Birth: [REDACTED]")]
        [InlineData("SSN: 123-45-6789", "SSN: [REDACTED]")]
        [InlineData("Phone Number: (123) 456-7890", "Phone Number: [REDACTED]")]
        [InlineData("Email: test@example.com", "Email: [REDACTED]")]
        public void RedactPhi_ValidInput_RedactsPhi(string input, string expected)
        {
            // Act
            var result = _phiRedactionService.RedactPhi(input);

            // Assert
            Assert.Equal(expected, result);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Redacting PHI from text")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("PHI redacted from text")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public void RedactPhi_ThrowsException_LogsError()
        {
            // Arrange
            var input = (string)null; // Null input to cause an exception

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _phiRedactionService.RedactPhi(input));
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while redacting PHI from text")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
