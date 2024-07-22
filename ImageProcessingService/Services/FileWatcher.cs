using ImageProcessingService.Interfaces;
using Microsoft.Extensions.Logging;

namespace ImageProcessingService.Services;

public class FileWatcher : IFileWatcher
{
    private FileSystemWatcher watcher;
    private readonly ILogger<FileWatcher> logger;

    public FileWatcher(ILogger<FileWatcher> logger)
    {
        this.logger = logger;
    }
    
    public void Initialize(string path)
    {
        watcher = new FileSystemWatcher(path);

        watcher.NotifyFilter = NotifyFilters.Attributes
                               | NotifyFilters.CreationTime
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.FileName
                               | NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Security
                               | NotifyFilters.Size;
        
        watcher.Created += OnCreatedFile;

        watcher.Filter = "*.*";
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
        this.logger.LogInformation("Start polling {0}", path);
    }

    private void OnCreatedFile(object sender, FileSystemEventArgs args)
    {
        OnCreated?.Invoke(this, args);
    }

    public event FileSystemEventHandler OnCreated;
}