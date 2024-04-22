using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Repositories
{
    public class MoneyAccountRepository : IMoneyAccountRepository
    {
        private readonly IDbContextFactory<MoneyContext> _contextFactory;
        public MoneyAccountRepository(IDbContextFactory<MoneyContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<MoneyAccount> Get(byte id)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.MoneyAccounts.FindAsync(id);
        }

        public async Task<IEnumerable<MoneyAccount>> GetAll()
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.MoneyAccounts.ToListAsync();
        }
    }
}