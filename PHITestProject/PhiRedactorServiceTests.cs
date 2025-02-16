using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PHIRedationApplication.Server.Services;
using PHIRedationApplication.Server.Services.Interfaces;

namespace PHITestProject;

public class PhiRedactorServiceTests
{
    private readonly Mock<IWriteFileService> _mockWriteFileService;
    private readonly Mock<IReadFileService> _mockReadFileService;
    private readonly Mock<IPhiRedactionService> _mockPhiRedactionService;
    private readonly Mock<ILogger<PhiRedactorService>> _mockLogger;
    private readonly PhiRedactorService _phiRedactorService;

    public PhiRedactorServiceTests()
    {
        _mockWriteFileService = new Mock<IWriteFileService>();
        _mockReadFileService = new Mock<IReadFileService>();
        _mockPhiRedactionService = new Mock<IPhiRedactionService>();
        _mockLogger = new Mock<ILogger<PhiRedactorService>>();
        _phiRedactorService = new PhiRedactorService(_mockWriteFileService.Object, _mockReadFileService.Object, _mockPhiRedactionService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task RedactPhiFromFile_ValidFile_RedactsPhi()
    {
        var fileMock = new Mock<IFormFile>();
        var content = "This is a test file content.";
        var redactedContent = "This is a [REDACTED] file content.";
        var fileName = "test.txt";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ProcessedFiles", $"_{fileName}");

        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);

        _mockReadFileService.Setup(rfs => rfs.ReadFileContentAsync(fileMock.Object)).ReturnsAsync(content);
        _mockPhiRedactionService.Setup(prs => prs.RedactPhi(content)).Returns(redactedContent);
        _mockWriteFileService.Setup(wfs => wfs.WriteFileContentAsync(redactedContent, fileName)).ReturnsAsync(filePath);

        var result = await _phiRedactorService.RedactPhiFromFile(fileMock.Object);

        Assert.Equal(filePath, result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("PHI redacted and file saved")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task RedactPhiFromFile_NullFile_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _phiRedactorService.RedactPhiFromFile(null));
    }

    [Fact]
    public async Task RedactPhiFromFile_ThrowsException_LogsError()
    {
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

        _mockReadFileService.Setup(rfs => rfs.ReadFileContentAsync(fileMock.Object)).Throws(new Exception("Test exception"));

        var exception = await Assert.ThrowsAsync<Exception>(() => _phiRedactorService.RedactPhiFromFile(fileMock.Object));
        Assert.Equal("Test exception", exception.Message);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error redacting PHI from file")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}