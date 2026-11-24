namespace GymManagementSystem.BLL.Common;

public record AttachmentFile(Stream Content, string ContentType);

public static class AttachmentFolders
{
    public const string Members = "members";
}
