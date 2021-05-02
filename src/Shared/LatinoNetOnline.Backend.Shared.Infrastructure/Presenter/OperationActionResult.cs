
using Microsoft.AspNetCore.Mvc;

using LatinoNetOnline.Backend.Shared.Abstractions.OperationResults;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Presenter
{
    public class OperationActionResult : ObjectResult
    {
        public OperationActionResult(OperationResult value) : base(value)
        {
            StatusCode = (int)value.Status;
        }
    }
}
