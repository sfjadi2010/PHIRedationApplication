using Microsoft.Extensions.Logging;
using Moq;
using PHIRedationApplication.Server.Services;
using PHIRedationApplication.Server.Services.Interfaces;

namespace PHITestProject
{
    public class WriteFileServiceTests
    {
        private readonly Mock<IDirectoryService> _mockDirectoryService;
        private readonly Mock<ILogger<WriteFileService>> _mockLogger;
        private readonly WriteFileService _writeFileService;

        public WriteFileServiceTests()
        {
            _mockDirectoryService = new Mock<IDirectoryService>();
            _mockLogger = new Mock<ILogger<WriteFileService>>();
            _writeFileService = new WriteFileService(_mockDirectoryService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task WriteFileContentAsync_ValidInput_WritesFile()
        {
            // Arrange
            var content = "This is a test file content.";
            var fileName = "test.txt";
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "ProcessedFiles");
            var filePath = Path.Combine(uploadDirectory, $"_{fileName}");

            // Ensure the directory exists before running the test
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            _mockDirectoryService.Setup(ds => ds.EnsureDirectoryExists(uploadDirectory)).Returns(uploadDirectory);

            // Act
            var result = await _writeFileService.WriteFileContentAsync(content, fileName);

            // Assert
            Assert.Equal(filePath, result);
            Assert.True(File.Exists(filePath));
            Assert.Equal(content, await File.ReadAllTextAsync(filePath));
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("File written successfully")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public async Task WriteFileContentAsync_ThrowsException_LogsError()
        {
            // Arrange
            var content = "This is a test file content.";
            var fileName = "test.txt";
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "ProcessedFiles");

            _mockDirectoryService.Setup(ds => ds.EnsureDirectoryExists(uploadDirectory)).Throws(new Exception("Test exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _writeFileService.WriteFileContentAsync(content, fileName));
            Assert.Equal("Test exception", exception.Message);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error writing file")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
