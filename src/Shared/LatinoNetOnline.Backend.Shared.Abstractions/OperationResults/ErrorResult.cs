﻿namespace LatinoNetOnline.Backend.Shared.Abstractions.OperationResults
{
    public class ErrorResult
    {
        public ErrorResult(string code)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
}
