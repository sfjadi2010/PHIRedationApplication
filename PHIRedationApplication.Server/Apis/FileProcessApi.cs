using Microsoft.AspNetCore.Mvc;
using PHIRedationApplication.Server.Services;

namespace PHIRedationApplication.Server.Apis;

public static class FileProcessApi
{
    public static WebApplication PostMaps(this WebApplication app)
    {
        app.MapPost("api/fileprocessor", async (HttpRequest request, [FromServices] PhiRedactorService phiRedatorService) =>
        {
            try
            {
                // Check if the request contains a file
                if (!request.HasFormContentType)
                {
                    return Results.BadRequest("Invalid request content type.");
                }

                // read the file from the request
                var form = await request.ReadFormAsync();
                var file = form.Files["file"];

                if (file == null || file.Length == 0)
                {
                    return Results.BadRequest("No file found in the request.");
                }

                var result = phiRedatorService.RedactPhiFromFile(file);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        });

        return app;
    }
}
