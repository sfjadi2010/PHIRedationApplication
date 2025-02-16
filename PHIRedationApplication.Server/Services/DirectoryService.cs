using PHIRedationApplication.Server.Services.Interfaces;

namespace PHIRedationApplication.Server.Services;

/// <summary>
/// Provides methods to ensure the existence of directories.
/// </summary>
public class DirectoryService : IDirectoryService
{
    private readonly ILogger<DirectoryService> _logger;

    public DirectoryService(ILogger<DirectoryService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Ensures that the specified directory exists. If the directory does not exist, it is created.
    /// </summary>
    /// <param name="directoryPath">The path of the directory to check or create.</param>
    /// <returns>The path of the directory.</returns>
    public string EnsureDirectoryExists(string directoryPath)
    {
        try
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                _logger.LogInformation("Directory created at path: {DirectoryPath}", directoryPath);
            }
            else
            {
                _logger.LogInformation("Directory already exists at path: {DirectoryPath}", directoryPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while ensuring the directory exists at path: {DirectoryPath}", directoryPath);
            throw;
        }

        return directoryPath;
    }
}
