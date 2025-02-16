namespace PHIRedationApplication.Server.Services;

public class ReadFileService
{
    private readonly ILogger<ReadFileService> _logger;

    public ReadFileService(ILogger<ReadFileService> logger)
    {
        _logger = logger;
    }

    public async Task<string> ReadFileContentAsync(IFormFile file)
    {
        try
        {
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
