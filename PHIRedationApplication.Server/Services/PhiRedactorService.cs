using PHIRedationApplication.Server.Services.Interfaces;

namespace PHIRedationApplication.Server.Services;

/// <summary>
/// Service for redacting PHI (Protected Health Information) from files.
/// </summary>
public class PhiRedactorService
{
    private readonly IWriteFileService _writeFileService;
    private readonly IReadFileService _readFileService;
    private readonly IPhiRedactionService _phiRedactionService;
    private readonly ILogger<PhiRedactorService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PhiRedactorService"/> class.
    /// </summary>
    /// <param name="writeFileService">The service to handle writing file content.</param>
    /// <param name="readFileService">The service to handle reading file content.</param>
    /// <param name="phiRedactionService">The service to handle PHI redaction.</param>
    /// <param name="logger">The logger instance to use for logging.</param>
    public PhiRedactorService(IWriteFileService writeFileService, IReadFileService readFileService, IPhiRedactionService phiRedactionService, ILogger<PhiRedactorService> logger)
    {
        _writeFileService = writeFileService;
        _readFileService = readFileService;
        _phiRedactionService = phiRedactionService;
        _logger = logger;
    }

    /// <summary>
    /// Redacts PHI from the provided file.
    /// </summary>
    /// <param name="file">The file to redact PHI from.</param>
    /// <returns>The path of the file with PHI redacted.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided file is null.</exception>
    /// <exception cref="Exception">Thrown when an error occurs during PHI redaction or file operations.</exception>
    public async Task<string> RedactPhiFromFile(IFormFile? file)
    {
        if (file == null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        try
        {
            // read file content using ReadFileService
            var content = await _readFileService.ReadFileContentAsync(file);

            // redact PHI from the content using PhiRedactionService
            content = _phiRedactionService.RedactPhi(content);

            // write redacted content to a new file using WriteFileService
            var result = await _writeFileService.WriteFileContentAsync(content, file.FileName);

            _logger.LogInformation("PHI redacted and file saved: {FileName}", file.FileName);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error redacting PHI from file {FileName}", file.FileName);
            throw;
        }
    }
}
