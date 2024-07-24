using ImageProcessingService.Models;

namespace ImageProcessingService.Interfaces;

public interface IFaceDetectionService
{
    Task InitializeAsync();
    Task ProcessImage(string imageDirectory);
}