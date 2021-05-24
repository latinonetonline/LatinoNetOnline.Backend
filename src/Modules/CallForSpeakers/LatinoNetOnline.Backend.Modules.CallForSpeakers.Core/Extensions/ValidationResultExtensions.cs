using CSharpFunctionalExtensions;

using FluentValidation.Results;

using System.Linq;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Extensions
{
    static class ValidationResultExtensions
    {
        public static Result<T> ToResult<T>(this ValidationResult validationResult, T successValue)
            => Result.SuccessIf(validationResult.IsValid, successValue, string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
    }
}
