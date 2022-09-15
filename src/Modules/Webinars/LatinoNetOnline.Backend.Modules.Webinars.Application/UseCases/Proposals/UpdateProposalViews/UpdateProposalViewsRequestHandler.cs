using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.UpdateProposalViews
{
    public class UpdateProposalViewsRequestHandler : IRequestHandler<UpdateProposalViewsRequest, Result>
    {
        private readonly IRepository<Proposal> _repository;

        public UpdateProposalViewsRequestHandler(IRepository<Proposal> repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateProposalViewsRequest request, CancellationToken cancellationToken)
        {
            var proposals = await _repository.Query(true).Where(x => request.Views.Select(v => v.Id).Contains(x.Id)).ToListAsync(cancellationToken);

            foreach (var item in proposals)
            {
                var views = request.Views.FirstOrDefault(x => x.Id == item.Id);

                if (views is not null && views.Views.HasValue)
                {
                    item.Views = views.Views.Value;
                }
            }

            await _repository.UpdateRangeAsync(proposals, cancellationToken);


            return true;
        }
    }
}
