using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Repositories
{
    public class BusinessOperationRepository : IBusinessOperationRepository
    {
        private readonly IDbContextFactory<MoneyContext> _contextFactory;
        public BusinessOperationRepository(IDbContextFactory<MoneyContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<BusinessOperation> Create(BusinessOperation operation)
        {
            var context = _contextFactory.CreateDbContext();
            await context.BusinessOperations.AddAsync(operation);
            await context.SaveChangesAsync();
            return operation;
        }

        public async Task Delete(int id)
        {
            var context = _contextFactory.CreateDbContext();
            BusinessOperation operationToDelete = await context.BusinessOperations.FindAsync(id);
            context.BusinessOperations.Remove(operationToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BusinessOperation>> GetAll()
        {
            var context = _contextFactory.CreateDbContext();
            return await context.BusinessOperations.ToListAsync();
        }

        public async Task<BusinessOperation> Get(int id)
        {
            var context = _contextFactory.CreateDbContext();
            return await context.BusinessOperations.FindAsync(id);
        }

        public async Task<BusinessOperationForShow> GetForShow(int id)
        {
            var context = _contextFactory.CreateDbContext();
            return await context.BusinessOperationsForShow.Where(op => op.BusinessOperationId == id).FirstAsync();
        }

        public async Task<IEnumerable<BusinessOperation>> GetByDay(DateTime dateTime)
        {
            var context = _contextFactory.CreateDbContext();
            return await context.BusinessOperations.Where(operation
                => operation.OperationDateTime.Date == dateTime.Date).ToListAsync();
        }

        public async Task<IEnumerable<BusinessOperationForShow>> GetByIncome(DateTime dateTime)
        {
            var context = _contextFactory.CreateDbContext();
            return await context.BusinessOperationsForShow.Where(op =>
            op.OperationDateTime.Date == dateTime.Date && op.IsIncome == true).ToListAsync();
        }

        public async Task<IEnumerable<BusinessOperationForShow>> GetByConsumption(DateTime dateTime)
        {
            var context = _contextFactory.CreateDbContext();
            return await context.BusinessOperationsForShow.Where(op =>
            op.OperationDateTime.Date == dateTime.Date && op.IsIncome == false).ToListAsync();
        }
        public async Task Update(BusinessOperation operation)
        {
            var context = _contextFactory.CreateDbContext();
            BusinessOperation businessOperationToUpdate = 
                await context.BusinessOperations.SingleOrDefaultAsync(t
                => t.BusinessOperationId == operation.BusinessOperationId);
            if (businessOperationToUpdate != null)
            {
                businessOperationToUpdate.OperationTypeId = operation.OperationTypeId;
                businessOperationToUpdate.OperationDateTime = operation.OperationDateTime;
                businessOperationToUpdate.Amount = operation.Amount;
                businessOperationToUpdate.Comment = operation.Comment;
                businessOperationToUpdate.MoneyAccountId = operation.MoneyAccountId;
                await context.SaveChangesAsync();
            }
        }
    }
}