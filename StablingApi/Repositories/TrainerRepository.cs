using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Repositories
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly IDbContextFactory<TrainersContext> _contextFactory;
        public TrainerRepository(IDbContextFactory<TrainersContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Trainer> Create(Trainer trainer)
        {
            TrainersContext context = await _contextFactory.CreateDbContextAsync();
            context.Add(trainer);
            await context.SaveChangesAsync();
            return trainer;
        }

        public async Task<IEnumerable<Trainer>> GetAll()
        {
            TrainersContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Trainers.ToListAsync();
        }

        public async Task<Trainer> Get(int id)
        {
            TrainersContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Trainers.FindAsync(id);
        }

        public async Task<IEnumerable<Trainer>> GetByAvailability(bool isAvailable)
        {
            TrainersContext context = await _contextFactory.CreateDbContextAsync();
            return await context.Trainers.Where(t => t.IsAvailable == isAvailable).ToListAsync();
        }
    }
}
