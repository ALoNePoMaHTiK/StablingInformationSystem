using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Repositories
{
    public class AbonementRepository : IAbonementRepository
    {

        private readonly IDbContextFactory<AbonementsContext> _contextFactory;
        public AbonementRepository(IDbContextFactory<AbonementsContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Abonement> Create(Abonement abonement)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            context.Add(abonement);
            await context.SaveChangesAsync();
            return abonement;
        }
        public async Task Delete(int id)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            Abonement abonementToDelete = await context.Abonements.FindAsync(id);
            context.Abonements.Remove(abonementToDelete);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Abonement>> GetAll()
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.Abonements.ToListAsync();
        }
        public async Task<Abonement> Get(int id)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.Abonements.FindAsync(id);
        }
        public async Task<IEnumerable<Abonement>> GetByClient(int clientId)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.Abonements.Where(a => a.ClientId == clientId).ToListAsync();
        }
        public async Task<IEnumerable<Abonement>> GetByType(int abonementTypeId)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.Abonements.Where(a => a.AbonementTypeId == abonementTypeId).ToListAsync();
        }

        public async Task<IEnumerable<Abonement>> Get(bool IsAvailable)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.Abonements.Where(a => a.IsAvailable == IsAvailable).ToListAsync();
        }
        public async Task Update(Abonement abonement)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            Abonement abonementToUpdate = await context.Abonements.FindAsync(abonement.AbonementId);
            if (abonementToUpdate != null)
            {
                abonementToUpdate.AbonementTypeId = abonement.AbonementTypeId;
                abonementToUpdate.ClientId = abonement.ClientId;
                abonementToUpdate.UsesCount = abonement.UsesCount;
                abonementToUpdate.OpenDateTime = abonement.OpenDateTime;
                abonementToUpdate.IsAvailable = abonement.IsAvailable;
            }
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Abonement>> GetAllNotPaid()
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.Abonements.Where(t => context.AbonementWithdrawings.Where(w => w.AbonementId == t.AbonementId).FirstOrDefault() == null).ToListAsync();
        }
    }
}