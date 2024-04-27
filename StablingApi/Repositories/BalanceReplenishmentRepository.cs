using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Repositories.Interfaces;
using StablingApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StablingApi.Repositories
{
    public class BalanceReplenishmentRepository : IBalanceReplenishmentRepository
    {
        private readonly IDbContextFactory<MoneyContext> _contextFactory;
        public BalanceReplenishmentRepository(IDbContextFactory<MoneyContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<BalanceReplenishment> Create(BalanceReplenishment replenishment)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            await context.AddAsync(replenishment);
            await context.SaveChangesAsync();
            return replenishment;
        }

        public async Task Delete(int id)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            BalanceReplenishment replenishmenToDelete = await context.BalanceReplenishments.FindAsync(id);
            context.BalanceReplenishments.Remove(replenishmenToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<BalanceReplenishment> Get(int id)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.BalanceReplenishments.FindAsync(id);
        }

        public async Task<IEnumerable<BalanceReplenishment>> GetAll()
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.BalanceReplenishments.ToListAsync();
        }

        public async Task<IEnumerable<BalanceReplenishment>> GetByDate(DateTime date)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.BalanceReplenishments.Where(r => r.ReplenishmentDate == date).ToListAsync();
        }

        public async Task<IEnumerable<BalanceReplenishmentForShow>> GetByDateForShow(DateTime date)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.BalanceReplenishmentsForShow.Where(br =>
            br.ReplenishmentDate.Date == date.Date).ToListAsync();
        }

        public async Task<BalanceReplenishmentForShow> GetForShow(int id)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.BalanceReplenishmentsForShow.Where(br =>
            br.BalanceReplenishmentId == id).FirstAsync();
        }

        public async Task Update(BalanceReplenishment replenishment)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            BalanceReplenishment replenishmentToUpdate = await context.BalanceReplenishments.SingleOrDefaultAsync(r => r.BalanceReplenishmentId == replenishment.BalanceReplenishmentId);
            if (replenishmentToUpdate != null)
            {
                replenishmentToUpdate.ClientId = replenishment.ClientId;
                replenishmentToUpdate.TrainerId = replenishment.TrainerId;
                replenishmentToUpdate.ReplenishmentDate = replenishment.ReplenishmentDate;
                replenishmentToUpdate.Amount = replenishment.Amount;
                await context.SaveChangesAsync();
            }
        }
    }
}