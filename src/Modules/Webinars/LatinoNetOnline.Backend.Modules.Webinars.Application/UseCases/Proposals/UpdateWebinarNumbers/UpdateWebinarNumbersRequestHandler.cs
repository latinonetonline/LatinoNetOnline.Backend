using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Enums;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.UpdateWebinarNumbers
{
    public class UpdateWebinarNumbersRequestHandler : IRequestHandler<UpdateWebinarNumbersRequest, Result>
    {
        private readonly IRepository<Proposal> _repository;


        public UpdateWebinarNumbersRequestHandler(IRepository<Proposal> repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateWebinarNumbersRequest request, CancellationToken cancellationToken)
        {
            var webinars = await _repository.FindAsync(x => x.EventDate.Date >= DateTime.Today, true, cancellationToken);


            if (!webinars.Any()) return "No hay propuestas";


            var lastWebinarNumberConfirmated = await _repository.Query(false)
                .Where(x => x.Status == WebinarStatus.Published && x.EventDate.Date < DateTime.Today)
                .OrderByDescending(x => x.EventDate)
                .Select(x => x.WebinarNumber)
                .FirstOrDefaultAsync();


            if (!lastWebinarNumberConfirmated.HasValue) return "No hay un último numero de webinar publicado.";


            webinars.UpdateWebinarNumber(lastWebinarNumberConfirmated.Value);

            await _repository.UpdateRangeAsync(webinars, cancellationToken);



            //foreach (var item in webinars)
            //{
            //    await _eventDispatcher.PublishAsync(new WebinarUpdatedEventInput(item.Id));
            //}

            return true;
        }
    }
}
