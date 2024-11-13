using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Repositories
{
    public class AbonementUsageRepository : IAbonementUsageRepository
    {
        private readonly IDbContextFactory<AbonementsContext> _contextFactory;
        public AbonementUsageRepository(IDbContextFactory<AbonementsContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<AbonementUsage> Create(AbonementUsage usage)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            await context.AbonementUsages.AddAsync(usage);
            await context.SaveChangesAsync();
            return usage;
        }

        public async Task<AbonementUsage> Create(int abonementId, int trainingId)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            AbonementUsage usage = new AbonementUsage() { AbonementId = abonementId, TrainingId = trainingId };
            await context.AbonementUsages.AddAsync(usage);
            await context.SaveChangesAsync();
            return usage;
        }

        public async Task Delete(int id)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            AbonementUsage usageToDelete = await context.AbonementUsages.FindAsync(id);
            context.AbonementUsages.Remove(usageToDelete);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<AbonementUsage>> Get()
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.AbonementUsages.ToListAsync();
        }
        public async Task<AbonementUsage> Get(int id)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.AbonementUsages.FindAsync(id);
        }

        public async Task<IEnumerable<AbonementUsage>> GetByAbonement(int abonementId)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.AbonementUsages.Where(u => u.AbonementId == abonementId).ToListAsync();
        }

        public async Task<AbonementUsage> GetByTraining(int trainingId)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.AbonementUsages.Where(u => u.TrainingId == trainingId).FirstAsync();
        }

        public async Task Update(AbonementUsage usage)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            context.Entry(usage).State = EntityState.Modified;
            AbonementUsage usageToUpdate = await context.AbonementUsages.FindAsync(usage.AbonementUsageId);
            if (usageToUpdate != null)
            {
                usageToUpdate.AbonementId = usage.AbonementId;
                usageToUpdate.TrainingId = usage.TrainingId;
            }
            await context.SaveChangesAsync();
        }
    }
}