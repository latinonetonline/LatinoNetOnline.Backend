using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.UnavailableDates;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;
using LatinoNetOnline.Backend.Modules.Events.Core.Validators;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface IUnavailableDateService
    {
        Task<OperationResult<IEnumerable<UnavailableDateDto>>> GetAllAsync();
        Task<OperationResult<UnavailableDateDto>> CreateAsync(CreateUnavailableDateInput input);
        Task<OperationResult<UnavailableDateDto>> UpdateAsync(UpdateUnavailableDateInput input);
        Task<OperationResult> DeleteAsync(Guid id);
    }


    internal class UnavailableDateService : IUnavailableDateService
    {
        private readonly ApplicationDbContext _dbContext;

        public UnavailableDateService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<UnavailableDateDto>> CreateAsync(CreateUnavailableDateInput input)
        {
            CreateUnavailableDateValidator validator = new(_dbContext);

            var validationResult = validator.Validate(input);

            if (validationResult.IsValid)
            {
                UnavailableDate entity = new(input.Date, input.Reason);

                await _dbContext.UnavailableDates.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                return OperationResult<UnavailableDateDto>.Success(new(entity.Id, entity.Date, entity.Reason));
            }

            return OperationResult<UnavailableDateDto>.Fail(new(string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage))));

        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.UnavailableDates.SingleOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return OperationResult<UnavailableDateDto>.Fail(new("No existe este registro."));
            }

            _dbContext.UnavailableDates.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return OperationResult.Success();
        }

        public async Task<OperationResult<IEnumerable<UnavailableDateDto>>> GetAllAsync()
        {
            var dates = await _dbContext.UnavailableDates.AsNoTracking()
                .Where(x => x.Date >= DateTime.Now)
                .Select(x => new UnavailableDateDto(x.Id, x.Date, x.Reason))
                .ToListAsync();

            return OperationResult<IEnumerable<UnavailableDateDto>>.Success(dates);
        }

        public async Task<OperationResult<UnavailableDateDto>> UpdateAsync(UpdateUnavailableDateInput input)
        {
            var entity = await _dbContext.UnavailableDates.SingleOrDefaultAsync(x => x.Id == input.Id);

            if (entity is null)
            {
                return OperationResult<UnavailableDateDto>.Fail(new("No existe este registro."));
            }

            CreateUnavailableDateValidator validator = new(_dbContext);

            var validationResult = validator.Validate(new CreateUnavailableDateInput(input.Date, input.Reason));

            if (validationResult.IsValid)
            {
                entity.Reason = input.Reason;
                entity.Date = input.Date;

                _dbContext.UnavailableDates.Update(entity);
                await _dbContext.SaveChangesAsync();

                return OperationResult<UnavailableDateDto>.Success(new(entity.Id, entity.Date, entity.Reason));
            }


            return OperationResult<UnavailableDateDto>.Fail(new(string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage))));
        }
    }
}
