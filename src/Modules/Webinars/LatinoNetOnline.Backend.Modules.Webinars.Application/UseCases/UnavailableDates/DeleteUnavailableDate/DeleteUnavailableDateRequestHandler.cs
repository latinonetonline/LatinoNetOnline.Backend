using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.DeleteUnavailableDate
{
    public class DeleteUnavailableDateRequestHandler : IRequestHandler<DeleteUnavailableDateRequest, Result>
    {

        private readonly IRepository<UnavailableDate> _repository;
        public DeleteUnavailableDateRequestHandler(IRepository<UnavailableDate> repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteUnavailableDateRequest request, CancellationToken cancellationToken)
        {
            var entity = await _repository.SingleOrDefaultAsync(x => x.Id == request.Id, true, cancellationToken);


            if (entity is null)
            {
                return "No existe";
            }

            await _repository.RemoveAsync(entity, cancellationToken);

            return true;
        }
    }
}
