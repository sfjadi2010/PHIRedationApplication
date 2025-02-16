namespace PHIRedationApplication.Server.Services.Interfaces;

public interface IReadFileService
{
    Task<string> ReadFileContentAsync(IFormFile file);
}
