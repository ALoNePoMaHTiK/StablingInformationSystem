using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Repositories
{
    public class AbonementMarkRepository : IAbonementMarkRepository
    {
        private readonly IDbContextFactory<AbonementsContext> _contextFactory;
        public AbonementMarkRepository(IDbContextFactory<AbonementsContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<AbonementMark> Create(AbonementMark mark)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            await context.AbonementMarks.AddAsync(mark);
            await context.SaveChangesAsync();
            return mark;
        }
        public async Task Delete(int id)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            AbonementMark markToDelete = await context.AbonementMarks.FindAsync(id);
            context.AbonementMarks.Remove(markToDelete);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<AbonementMark>> Get()
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.AbonementMarks.ToListAsync();
        }
        public async Task<AbonementMark> Get(int id)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.AbonementMarks.FindAsync(id);
        }

        public async Task<IEnumerable<AbonementMark>> GetByAbonement(int abonementId)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.AbonementMarks.Where(mark => mark.AbonementId == abonementId).ToListAsync();
        }

        public async Task<AbonementMark> GetByTraining(int trainingId)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.AbonementMarks.Where(mark => mark.TrainingId == trainingId).FirstAsync();
        }

        public async Task Update(AbonementMark mark)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            context.Entry(mark).State = EntityState.Modified;
            AbonementMark markToUpdate = await context.AbonementMarks.FindAsync(mark.AbonementMarkId);
            if (markToUpdate != null)
            {
                markToUpdate.AbonementId = mark.AbonementId;
                markToUpdate.TrainingId = mark.TrainingId;
            }
            await context.SaveChangesAsync();
        }
    }
}