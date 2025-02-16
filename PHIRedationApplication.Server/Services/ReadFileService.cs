using PHIRedationApplication.Server.Services.Interfaces;

namespace PHIRedationApplication.Server.Services;

/// <summary>
/// Service to read the content of a file.
/// </summary>
public class ReadFileService : IReadFileService
{
    private readonly ILogger<ReadFileService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadFileService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public ReadFileService(ILogger<ReadFileService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Reads the content of the specified file asynchronously.
    /// </summary>
    /// <param name="file">The file to read.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains the file content as a string.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while reading the file.</exception>
    public async Task<string> ReadFileContentAsync(IFormFile file)
    {
        try
        {
            // read file content
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var content = await reader.ReadToEndAsync();
                _logger.LogInformation("File read successfully: {FileName}", file.FileName);
                return content;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading file {FileName}", file.FileName);
            throw;
        }
    }
}
