namespace PHIRedationApplication.Server.Services.Interfaces;

public interface IWriteFileService
{
    Task<string> WriteFileContentAsync(string content, string filePath);
}
