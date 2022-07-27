using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions
{
    static class ResultExtensions
    {
        public static OperationResult<T> ToOperationResult<T>(this Result<T> result) where T : class
            => result.IsSuccess ? OperationResult<T>.Success(result.Value) :
                  OperationResult<T>.Fail(new(result.Error));

        public static Task<OperationResult<T>> FinallyOperationResult<T>(this Task<Result<T>> result) where T : class
            => result.Finally(result => result.ToOperationResult());


        public static OperationResult ToOperationResult(this Result result)
            => result.IsSuccess ? OperationResult.Success() :
          OperationResult.Fail(new(result.Error));

        public static Task<OperationResult> FinallyOperationResult(this Task<Result> result)
            => result.Finally(result => result.ToOperationResult());

    }
}
