using System.Net;
using System.Text.Json.Serialization;

namespace LatinoNetOnline.Backend.Shared.Commons.OperationResults
{
    public class OperationResult
    {
        [JsonIgnore]
        public HttpStatusCode Status { get; set; }

        public bool IsSuccess { get; set; }

        public ErrorResult Error { get; set; }

        public static OperationResult Success() =>

            new()
            {
                Status = HttpStatusCode.OK,
                IsSuccess = true
            };


        public static OperationResult Fail() =>

             new()
             {
                 Status = HttpStatusCode.BadRequest,
                 IsSuccess = false
             };

        public static OperationResult Fail(ErrorResult error) =>

             new()
             {
                 Error = error,
                 Status = HttpStatusCode.BadRequest,
                 IsSuccess = false
             };

        public static OperationResult ServerError() =>

             new()
             {
                 Status = HttpStatusCode.InternalServerError,
                 IsSuccess = false
             };

        public static OperationResult Unauthorized() =>

             new()
             {
                 Status = HttpStatusCode.Unauthorized,
                 IsSuccess = false
             };

        public static OperationResult ServerError(ErrorResult error) =>

             new()
             {
                 Error = error,
                 Status = HttpStatusCode.InternalServerError,
                 IsSuccess = false
             };

    }
}
