﻿using LatinoNetOnline.Backend.Shared.Commons.Results;

using MediatR;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Website.GetNextWebinars
{
    public record GetNextWebinarsRequest(string Name) : IRequest<Result<GetNextWebinarsResponse?>>;

}