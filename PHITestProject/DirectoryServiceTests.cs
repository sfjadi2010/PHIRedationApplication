using Microsoft.Extensions.Logging;
using Moq;
using PHIRedationApplication.Server.Services;

namespace PHITestProject;
public class DirectoryServiceTests
{
    private readonly Mock<ILogger<DirectoryService>> _mockLogger;
    private readonly DirectoryService _directoryService;

    public DirectoryServiceTests()
    {
        _mockLogger = new Mock<ILogger<DirectoryService>>();
        _directoryService = new DirectoryService(_mockLogger.Object);
    }

    [Fact]
    public void EnsureDirectoryExists_DirectoryDoesNotExist_CreatesDirectory()
    {
        // Arrange
        var directoryPath = "test_directory";
        if (Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath, true);
        }

        // Act
        var result = _directoryService.EnsureDirectoryExists(directoryPath);

        // Assert
        Assert.True(Directory.Exists(directoryPath));
        Assert.Equal(directoryPath, result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Directory created at path")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

        // Cleanup
        Directory.Delete(directoryPath, true);
    }

    [Fact]
    public void EnsureDirectoryExists_DirectoryExists_DoesNotCreateDirectory()
    {
        // Arrange
        var directoryPath = "test_directory";
        Directory.CreateDirectory(directoryPath);

        // Act
        var result = _directoryService.EnsureDirectoryExists(directoryPath);

        // Assert
        Assert.True(Directory.Exists(directoryPath));
        Assert.Equal(directoryPath, result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Directory already exists at path")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

        // Cleanup
        Directory.Delete(directoryPath, true);
    }

    [Fact]
    public void EnsureDirectoryExists_ThrowsException_LogsError()
    {
        // Arrange
        var directoryPath = string.Empty; // Invalid path to cause an exception

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _directoryService.EnsureDirectoryExists(directoryPath));
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while ensuring the directory exists at path")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}
