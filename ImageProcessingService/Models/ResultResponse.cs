namespace ImageProcessingService.Models;

public record ResultResponse
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
}