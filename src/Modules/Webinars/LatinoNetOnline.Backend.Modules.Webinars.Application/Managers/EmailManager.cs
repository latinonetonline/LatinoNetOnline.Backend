﻿using CSharpFunctionalExtensions;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Emails;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Managers
{
    interface IEmailManager
    {
        Task<Result> SendEmailAsync(SendEmailInput input);
    }

}
