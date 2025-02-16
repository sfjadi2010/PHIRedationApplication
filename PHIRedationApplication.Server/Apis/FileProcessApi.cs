using Microsoft.AspNetCore.Mvc;
using PHIRedationApplication.Server.Services;

namespace PHIRedationApplication.Server.Apis;

public static class FileProcessApi
{
    public static WebApplication PostMaps(this WebApplication app)
    {
        app.MapPost("api/fileprocessor", async (HttpRequest request, ILogger logger, [FromServices] PhiRedactorService phiRedatorService) =>
        {
            try
            {
                logger.LogInformation("File processing request received.");

                logger.LogInformation("File processing completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the file.");
                throw;
            }
        });

        return app;
    }
}
