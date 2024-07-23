namespace ImageProcessingService.Models;

public record ResultResponse<T> 
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }
}

public static class ResultResponseExtensions
{
    public static ResultResponse<T> CreateSuccessfulResponse<T>(T data)
    {
        return new ResultResponse<T>()
        {
            IsSuccess = true,
            Data = data
        };
    }

    public static ResultResponse<T> CreateFailedResponse<T>(string errorMessage)
    {
        return new ResultResponse<T>()
        {
            ErrorMessage = errorMessage
        };
    }
}