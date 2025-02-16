using System.Text.RegularExpressions;

namespace PHIRedationApplication.Server.Services;

/// <summary>
/// Service for redacting PHI (Protected Health Information) from text content.
/// </summary>
public class PhiRedactionService
{
    private readonly ILogger<PhiRedactionService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PhiRedactionService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance to use for logging.</param>
    public PhiRedactionService(ILogger<PhiRedactionService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Redacts PHI from the provided text content.
    /// </summary>
    /// <param name="content">The text content to redact PHI from.</param>
    /// <returns>The text content with PHI redacted.</returns>
    public string RedactPhi(string content)
    {
        try
        {
            _logger.LogInformation("Redacting PHI from text: {Text}", content);

            // Redact PHI from the text
            var RedactedContent = Redact(content);
            _logger.LogInformation("PHI redacted from text.");
            return RedactedContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while redacting PHI from text.");
            throw;
        }
    }

    /// <summary>
    /// Redacts PHI from the provided text content based on predefined patterns.
    /// </summary>
    /// <param name="content">The text content to redact PHI from.</param>
    /// <returns>The text content with PHI redacted.</returns>
    private string Redact(string content)
    {
        var patterns = new Dictionary<string, string>
            {
                { "Name", @"(?<=Name:\s)(?<Value>\b[A-Z][a-z]*\s[A-Z][a-z]*\b)" },
                { "Patient Name", @"(?<=Patient Name:\s)(?<Value>\b[A-Z][a-z]*\s[A-Z][a-z]*\b)" },
                { "Date of Birth", @"(?<=Date of Birth:\s)(?<Value>\b\d{2}/\d{2}/\d{4}\b)" },
                { "DOB", @"(?<=DOB:\s)(?<Value>\b\d{2}/\d{2}/\d{4}\b)" },
                { "Social Security Number", @"(?<=Social Security Number:\s)(?<Value>\b\d{3}-\d{2}-\d{4}\b)" },
                { "SSN", @"(?<=SSN:\s)(?<Value>\b\d{3}-\d{2}-\d{4}\b)" },
                { "Phone Number", @"(?<=Phone Number:\s)(?<Value>\(\d{3}\)\s*\d{3}[-.\s]?\d{4}|\d{3}[-.\s]?\d{3}[-.\s]?\d{4}|\+\d{1,2}\s*\d{3}[-.\s]?\d{3}[-.\s]?\d{4})" },
                { "Home Number", @"(?<=Home Number:\s)(?<Value>\(\d{3}\)\s*\d{3}[-.\s]?\d{4}|\d{3}[-.\s]?\d{3}[-.\s]?\d{4}|\+\d{1,2}\s*\d{3}[-.\s]?\d{3}[-.\s]?\d{4})" },
                { "Cell Number", @"(?<=Cell Number:\s)(?<Value>\(\d{3}\)\s*\d{3}[-.\s]?\d{4}|\d{3}[-.\s]?\d{3}[-.\s]?\d{4}|\+\d{1,2}\s*\d{3}[-.\s]?\d{3}[-.\s]?\d{4})" },
                { "Fax Number", @"(?<=Fax Number:\s)(?<Value>\(\d{3}\)\s*\d{3}[-.\s]?\d{4}|\d{3}[-.\s]?\d{3}[-.\s]?\d{4}|\+\d{1,2}\s*\d{3}[-.\s]?\d{3}[-.\s]?\d{4})" },
                { "Email", @"(?<=Email:\s)(?<Value>\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b)" },
                { "Address", @"(?<=Address:\s)(?<Value>.*)" },
                { "Medical Record Number", @"(?<=Medical Record Number:\s)(?<Value>.*)" }
            };

        foreach (var pattern in patterns)
        {
            content = Regex.Replace(content, pattern.Value, "[REDACTED]", RegexOptions.Compiled);
        }

        return content;
    }
}
