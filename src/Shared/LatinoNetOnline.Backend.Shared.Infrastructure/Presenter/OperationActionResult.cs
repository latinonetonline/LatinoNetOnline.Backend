
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.AspNetCore.Mvc;

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
