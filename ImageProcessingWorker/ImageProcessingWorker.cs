using ImageProcessingService.Interfaces;

namespace ImageProcessingWorker;

public class ImageProcessingWorker : IHostedService
{
    private readonly IFaceDetectionService _faceDetectionService;
    private IFileWatcher fileWatcher;
    private readonly ILogger<ImageProcessingWorker> logger;

    public ImageProcessingWorker(
        IFaceDetectionService faceDetectionService,
        IFileWatcher fileWatcher,
        ILogger<ImageProcessingWorker> logger)
    {
        this._faceDetectionService = faceDetectionService;
        this.fileWatcher = fileWatcher;
        this.logger = logger;
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        this._faceDetectionService.Initialize();
        var testDirectory = Path.Combine(Environment.CurrentDirectory, "test");
        if (!Directory.Exists(testDirectory))
        {
            Directory.CreateDirectory(testDirectory);
        }
        this.fileWatcher.Initialize(testDirectory);
        this.fileWatcher.OnCreated += OnCreated;
        await ExecuteAsync(cancellationToken);
    }

    private void OnCreated(object sender, FileSystemEventArgs args)
    {
        this.logger.LogInformation("File {0} was created", args.FullPath);
        this.logger.LogInformation("Start processing file: {0}", args.Name);
        this._faceDetectionService.ProcessImage(args.FullPath);
    }
    
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // while (!stoppingToken.IsCancellationRequested)
        // {
        //     await Task.Delay(1000, stoppingToken);
        // }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}