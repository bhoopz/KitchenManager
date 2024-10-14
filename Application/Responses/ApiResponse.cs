using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; private set; }
        public List<ApiError> Errors { get; private set; }
        public T Result { get; private set; }

        protected ApiResponse(bool succeeded, List<ApiError> errors, T result)
        {
            Succeeded = succeeded;
            Errors = errors;
            Result = result;
        }

        public static ApiResponse<T> Success()
        {
            return new ApiResponse<T>(true, new List<ApiError>(), default);
        }

        public static ApiResponse<T> Success(T result)
        {
            return new ApiResponse<T>(true, new List<ApiError>(), result);
        }

        public static ApiResponse<T> Failure(List<ApiError> errors)
        {
            return new ApiResponse<T>(false, errors, default);
        }

        public static ApiResponse<T> Failure(ApiError error)
        {
            return new ApiResponse<T>(false, new List<ApiError> { error }, default);
        }
    }
}
