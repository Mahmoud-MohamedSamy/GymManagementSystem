using GymManagementSystem.BLL.Common;
using Microsoft.AspNetCore.Http;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IAttachmentService
{
    Result<string> Upload(string folderName, IFormFile file);

    bool Delete(string fileName, string folderName);

    Result<AttachmentFile> GetFile(string fileName, string folderName);
}
