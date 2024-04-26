using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Repositories.Interfaces;
using StablingApi.Models;

namespace StablingApi.Repositories
{
    public class BusinessOperationTypeRepository : IBusinessOperationTypeRepository
    {
        private readonly IDbContextFactory<MoneyContext> _contextFactory;
        public BusinessOperationTypeRepository(IDbContextFactory<MoneyContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<BusinessOperationType> Create(BusinessOperationType type)
        {
            var context = _contextFactory.CreateDbContext();
            await context.BusinessOperationTypes.AddAsync(type);
            await context.SaveChangesAsync();
            return type;
        }

        public async Task<BusinessOperationType> Get(int id)
        {
            var context = _contextFactory.CreateDbContext();
            return await context.BusinessOperationTypes.FindAsync(id);
        }

        public async Task<IEnumerable<BusinessOperationType>> GetAll()
        {
            var context = _contextFactory.CreateDbContext();
            return await context.BusinessOperationTypes.ToListAsync();
        }

        public async Task<IEnumerable<BusinessOperationType>> GetIncomeTypes()
        {
            var context = _contextFactory.CreateDbContext();
            return await context.BusinessOperationTypes.Where(type => type.IsIncome == true).ToListAsync();
        }

        public async Task<IEnumerable<BusinessOperationType>> GetConsumptionTypes()
        {
            var context = _contextFactory.CreateDbContext();
            return await context.BusinessOperationTypes.Where(type => type.IsIncome == false).ToListAsync();
        }

        public async Task Update(BusinessOperationType type)
        {
            var context = _contextFactory.CreateDbContext();
            BusinessOperationType typeToUpdate = await context.BusinessOperationTypes.FindAsync(type.BusinessOperationTypeId);
            if (typeToUpdate != null)
            {
                typeToUpdate.TypeName = type.TypeName;
                typeToUpdate.TypeAmount = type.TypeAmount;
                typeToUpdate.IsIncome = type.IsIncome;
            }
            await context.SaveChangesAsync();
        }
    }
}
