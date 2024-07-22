namespace ImageProcessingService.Interfaces;

public interface IFaceDetectionService
{
    void Initialize();
    Task<object> ProcessImage(string filePath);
}