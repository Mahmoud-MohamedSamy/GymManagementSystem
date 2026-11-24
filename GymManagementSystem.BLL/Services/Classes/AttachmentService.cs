using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GymManagementSystem.BLL.Services.Classes;


public class AttachmentService : IAttachmentService
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
    private const long MaxFileSizeInBytes = 5 * 1024 * 1024;
    private readonly string _rootPath;

    public AttachmentService(IWebHostEnvironment environment)
    {
        _rootPath = Path.Combine(environment.ContentRootPath, "Files");
    }

    public Result<string> Upload(string folderName, IFormFile file)
    {
        if (file is null || file.Length == 0)
            return Result.Fail<string>("Please choose a photo.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return Result.Fail<string>("Only .jpg, .jpeg and .png photos are allowed.");

        if (file.Length > MaxFileSizeInBytes)
            return Result.Fail<string>("The photo must be 5 MB or smaller.");

        if (!IsSafeName(folderName))
            return Result.Fail<string>("The photo could not be saved.");

        try
        {
            var folderPath = Path.Combine(_rootPath, folderName);
            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return Result.Ok(fileName);
        }
        catch (Exception)
        {
            return Result.Fail<string>("The photo could not be saved.");
        }
    }

    public bool Delete(string fileName, string folderName)
    {
        if (!IsSafeName(fileName) || !IsSafeName(folderName))
            return false;

        try
        {
            var filePath = Path.Combine(_rootPath, folderName, fileName);
            if (!File.Exists(filePath))
                return false;

            File.Delete(filePath);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<AttachmentFile> GetFile(string fileName, string folderName)
    {
        if (!IsSafeName(fileName) || !IsSafeName(folderName))
            return Result.Fail<AttachmentFile>("File not found.", ResultStatus.NotFound);

        var filePath = Path.Combine(_rootPath, folderName, fileName);
        if (!File.Exists(filePath))
            return Result.Fail<AttachmentFile>("File not found.", ResultStatus.NotFound);

        try
        {
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Result.Ok(new AttachmentFile(stream, GetContentType(fileName)));
        }
        catch (Exception)
        {
            return Result.Fail<AttachmentFile>("File not found.", ResultStatus.NotFound);
        }
    }

    private static bool IsSafeName(string? name)
        => !string.IsNullOrWhiteSpace(name) && name == Path.GetFileName(name);

    private static string GetContentType(string fileName)
        => Path.GetExtension(fileName).ToLowerInvariant() == ".png" ? "image/png" : "image/jpeg";
}
