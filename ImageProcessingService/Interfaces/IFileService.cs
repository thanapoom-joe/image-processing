using ImageProcessingService.Models;

namespace ImageProcessingService.Interfaces
{
    public interface IFileService
    {
        bool FileExist(string filePath);
        ResultResponse CreateFileIfNotExist(string filePath);
        ResultResponse DeleteFile(string filePath);
        string PathCombine(params string[] paths);
    }
}
