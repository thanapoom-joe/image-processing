using ImageProcessingService.Interfaces;
using ImageProcessingService.Services;
using ImageProcessingService.Settings;

namespace ImageProcessingWorker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<ImageProcessingWorker>();
        ConfigureServices(builder);

        var host = builder.Build();
        host.Run();
    }

    private static void ConfigureServices(HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IFileWatcher, FileWatcher>();
        builder.Services.AddTransient<IFaceDetectionService, FaceDetectionService>();
    }
}