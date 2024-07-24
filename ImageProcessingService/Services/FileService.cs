using ImageProcessingService.Interfaces;
using ImageProcessingService.Models;
using Microsoft.Extensions.Logging;

namespace ImageProcessingService.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger _logger;

        public FileService(
            ILogger<FileService> logger)
        {
            _logger = logger;
        }

        public ResultResponse CreateFileIfNotExist(string filePath)
        {
            if (File.Exists(filePath))
            {
                return ResultResponseExtensions.CreateSuccessResponse();
            }

            try
            {
                var stream = File.Create(filePath);
                return stream is not null ?
                    ResultResponseExtensions.CreateSuccessResponse() :
                    ResultResponseExtensions.CreateFailureResponse();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "An exception has occured while creating file {0}", filePath);
                return ResultResponseExtensions.CreateFailureResponse(ex.Message);
            }

        }

        public ResultResponse DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  <inheritdoc/>
        /// </summary>        
        public bool FileExist(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>        
        public string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }
    }
}
