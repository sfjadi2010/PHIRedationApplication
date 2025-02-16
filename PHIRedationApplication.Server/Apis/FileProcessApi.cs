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

                // Check if the request contains a file
                if (!request.HasFormContentType)
                {
                    logger.LogError("Invalid request content type.");
                    return Results.BadRequest("Invalid request content type.");
                }

                // read the file from the request
                var form = await request.ReadFormAsync();
                var file = form.Files["file"];

                if (file == null || file.Length == 0)
                {
                    logger.LogError("No file found in the request.");
                    return Results.BadRequest("No file found in the request.");
                }

                var result = phiRedatorService.RedactPhiFromFile(file);

                logger.LogInformation("File processing completed successfully.");
                return Results.Ok(result);
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
