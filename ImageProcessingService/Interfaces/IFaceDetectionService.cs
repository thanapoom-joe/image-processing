using ImageProcessingService.Models;

namespace ImageProcessingService.Interfaces;

public interface IFaceDetectionService
{
    void Initialize();
    Task ProcessImage(string imageDirectory);
}