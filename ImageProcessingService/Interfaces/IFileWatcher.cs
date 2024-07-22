namespace ImageProcessingService.Interfaces;

public interface IFileWatcher
{
    void Initialize(string path);
    event FileSystemEventHandler OnCreated;
}