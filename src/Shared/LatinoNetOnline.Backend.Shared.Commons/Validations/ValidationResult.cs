using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

namespace LatinoNetOnline.Backend.Shared.Commons.Validations
{
    public class ValidationResult
    {
        public ValidationResult(ErrorResult error)
        {
            Error = error;
        }

        public ValidationResult()
        {
        }

        public ErrorResult Error { get; private set; }

        public bool IsValid { get => Error is null || string.IsNullOrWhiteSpace(Error.Code); }
    }
}
