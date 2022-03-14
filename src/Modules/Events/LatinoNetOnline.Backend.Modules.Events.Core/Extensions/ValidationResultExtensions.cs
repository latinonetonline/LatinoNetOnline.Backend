
using FluentValidation.Results;

using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System.Linq;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Extensions
{
    static class ValidationResultExtensions
    {
        public static OperationResult<T> ToOperationResult<T>(this ValidationResult validationResult) where T : class
            => new OperationResult<T> { IsSuccess = validationResult.IsValid, Message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)) };

        public static OperationResult<T> ToOperationResult<T>(this ValidationResult validationResult, T successValue) where T : class
            => new OperationResult<T> { IsSuccess = validationResult.IsValid, Result = successValue, Message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)) };
    }
}
