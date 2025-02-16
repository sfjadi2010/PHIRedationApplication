namespace PHIRedationApplication.Server.Services.Interfaces;

public interface IDirectoryService
{
    string EnsureDirectoryExists(string directoryPath);
}
