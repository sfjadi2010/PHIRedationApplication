using PHIRedationApplication.Server.Services.Interfaces;

namespace PHIRedationApplication.Server.Services;

/// <summary>
/// Service to handle writing file content to the server.
/// </summary>
public class WriteFileService : IWriteFileService
{
    private readonly IDirectoryService _directoryService;
    private readonly ILogger<WriteFileService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WriteFileService"/> class.
    /// </summary>
    /// <param name="directoryService">The directory service to ensure directories exist.</param>
    /// <param name="logger">The logger to log information and errors.</param>
    public WriteFileService(IDirectoryService directoryService, ILogger<WriteFileService> logger)
    {
        _directoryService = directoryService;
        _logger = logger;
    }

    /// <summary>
    /// Writes the specified content to a file asynchronously.
    /// </summary>
    /// <param name="content">The content to write to the file.</param>
    /// <param name="fileName">The name of the file to write the content to.</param>
    /// <returns>The path of the written file.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while writing the file.</exception>
    public async Task<string> WriteFileContentAsync(string content, string fileName)
    {
        try
        {
            // Ensure the directory exists, if not creating it
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "ProcessedFiles");
            _directoryService.EnsureDirectoryExists(uploadDirectory);

            // Write the content to a file
            var filePath = Path.Combine(uploadDirectory, $"_{fileName}");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await File.WriteAllTextAsync(filePath, content);

            _logger.LogInformation("File written successfully to {FilePath}", filePath);
            return filePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error writing file {FileName}", fileName);
            throw;
        }
    }
}
