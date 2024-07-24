using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingService.Models
{
    public record ResultResponseWithData<T> : ResultResponse
    {
        public T Data { get; set; }
    }


    public static class ResultResponseExtensions
    {
        public static ResultResponse CreateSuccessResponse()
        {
            return new ResultResponse()
            {
                IsSuccess = true
            };
        }
        
        public static ResultResponse CreateFailureResponse(string errorMessage = null) => new ResultResponse { ErrorMessage = errorMessage };

        public static ResultResponseWithData<T> CreateSuccessfulResponse<T>(T data)
        {
            return new ResultResponseWithData<T>()
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static ResultResponseWithData<T> CreateFailedResponse<T>(string errorMessage)
        {
            return new ResultResponseWithData<T>()
            {
                ErrorMessage = errorMessage
            };
        }
    }
}
