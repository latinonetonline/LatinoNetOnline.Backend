﻿using System.Net;

namespace LatinoNetOnline.Backend.Shared.Commons.OperationResults
{
    public class OperationResult<T> : OperationResult where T : class
    {
        public OperationResult()
        {
        }

        public OperationResult(T result)
        {
            Result = result;
            IsSuccess = true;
        }

        public T Result { get; set; }

        public static OperationResult<T> Success(T result)
        {
            return new()
            {
                IsSuccess = true,
                Result = result,
                Status = HttpStatusCode.OK
            };
        }

        public static new OperationResult<T> Fail(ErrorResult error) =>

             new()
             {
                 Error = error,
                 Status = HttpStatusCode.BadRequest,
                 IsSuccess = false
             };


        public static OperationResult<T> NotFound(ErrorResult error) =>

             new()
             {
                 Error = error,
                 Status = HttpStatusCode.NotFound,
                 IsSuccess = false
             };

        public static OperationResult<T> NotFound() =>

             new()
             {
                 Status = HttpStatusCode.NotFound,
                 IsSuccess = false
             };


        public static new OperationResult<T> Unauthorized() =>

             new()
             {
                 Status = HttpStatusCode.Unauthorized,
                 IsSuccess = false
             };
    }
}
