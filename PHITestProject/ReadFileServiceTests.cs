using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PHIRedationApplication.Server.Services;

namespace PHITestProject
{
    public class ReadFileServiceTests
    {
        private readonly Mock<ILogger<ReadFileService>> _mockLogger;
        private readonly ReadFileService _readFileService;

        public ReadFileServiceTests()
        {
            _mockLogger = new Mock<ILogger<ReadFileService>>();
            _readFileService = new ReadFileService(_mockLogger.Object);
        }

        [Fact]
        public async Task ReadFileContentAsync_ValidFile_ReturnsContent()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "This is a test file content.";
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            // Act
            var result = await _readFileService.ReadFileContentAsync(fileMock.Object);

            // Assert
            Assert.Equal(content, result);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("File read successfully")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task ReadFileContentAsync_ThrowsException_LogsError()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var fileName = "test.txt";

            fileMock.Setup(_ => _.OpenReadStream()).Throws(new Exception("Test exception"));
            fileMock.Setup(_ => _.FileName).Returns(fileName);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _readFileService.ReadFileContentAsync(fileMock.Object));
            Assert.Equal("Test exception", exception.Message);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error reading file")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
