namespace BlogApi.Interfaces;

public interface IFileService
{
    Task<string> SaveFileToWwwrootAsync(IFormFile logoFile, string folderName);
}