using ImageProcessingService.Extensions;
using ImageProcessingService.Initializers;
using ImageProcessingService.Interfaces;
using ImageProcessingService.Services;

namespace ImageProcessingWorker;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<ImageProcessingWorker>();
        ConfigureServices(builder);

        var host = builder.Build();
        await host.InitAsync();
        host.Run();
    }

    private static void ConfigureServices(HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IFileWatcher, FileWatcher>();
        builder.Services.AddTransient<IFaceDetectionService, FaceDetectionService>();
        builder.Services.AddTransient<IFileService, FileService>();
        builder.Services.AddAsyncInitializer<FaceDetectionInitializer>();
    }
}