using ImageProcessingService.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImageProcessingService.Extensions
{
    /// <summary>
    /// Provides extension methods to perform async initialization of an application.
    /// </summary>
    public static class AsyncInitializationWebHostExtensions
    {
        /// <summary>
        /// Initializes the application, by calling all registered async initializers.
        /// </summary>
        /// <param name="host">The web host.</param>
        /// <returns>A task that represents the initialization completion.</returns>
        public static async Task InitAsync(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var rootInitializer = scope.ServiceProvider.GetService<RootInitializer>();
                if (rootInitializer == null)
                {
                    throw new InvalidOperationException("The async initialization service isn't registered, register it by calling AddAsyncInitialization() on the service collection or by adding an async initializer.");
                }

                await rootInitializer.InitializeAsync();
            }
        }
    }
}
