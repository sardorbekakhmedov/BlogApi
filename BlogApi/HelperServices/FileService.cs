using BlogApi.Interfaces;

namespace BlogApi.HelperServices;

public class FileService : IFileService
{
    private const string RootFolderName = "wwwroot";

    public async Task<string> SaveFileToWwwrootAsync(IFormFile logoFile, string folderName)
    {
        return await SaveFileAsync(logoFile, folderName);
    }

    public void DeleteFile(string filePath)
    {
        if (File.Exists(Path.Combine(Environment.CurrentDirectory, RootFolderName, filePath)))
        {
            File.Delete(Path.Combine(Environment.CurrentDirectory, RootFolderName, filePath));
        }
    }


    private void CheckDirectory(string folderName)
    {
        if (!Directory.Exists(Path.Combine(RootFolderName, folderName)))
        {
            Directory.CreateDirectory(Path.Combine(RootFolderName, folderName));
        }
    }

    private async Task<string> SaveFileAsync(IFormFile iFormFile, string folderName)
    {
        CheckDirectory(folderName);

        var newFileName = Guid.NewGuid() + Path.GetExtension(iFormFile.FileName);

        using (var ms = new MemoryStream())
        {
            await iFormFile.CopyToAsync(ms);
            await File.WriteAllBytesAsync(Path.Combine(RootFolderName, folderName, newFileName), ms.ToArray());
        }

        return $"/{folderName}/{newFileName}";
    }

}