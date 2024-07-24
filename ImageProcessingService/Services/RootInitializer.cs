﻿using ImageProcessingService.Interfaces;
using Microsoft.Extensions.Logging;

namespace ImageProcessingService.Services
{
    public class RootInitializer
    {
        private readonly ILogger<RootInitializer> _logger;
        private readonly IEnumerable<IAsyncInitializer> _initializers;

        public RootInitializer(ILogger<RootInitializer> logger, IEnumerable<IAsyncInitializer> initializers)
        {
            _logger = logger;
            _initializers = initializers;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("Starting async initialization");

            try
            {
                foreach (var initializer in _initializers)
                {
                    _logger.LogInformation("Starting async initialization for {InitializerType}", initializer.GetType());
                    try
                    {
                        await initializer.InitializeAsync();
                        _logger.LogInformation("Async initialization for {InitializerType} completed", initializer.GetType());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Async initialization for {InitializerType} failed", initializer.GetType());
                        throw;
                    }
                }

                _logger.LogInformation("Async initialization completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Async initialization failed");
                throw;
            }
        }
    }
}
