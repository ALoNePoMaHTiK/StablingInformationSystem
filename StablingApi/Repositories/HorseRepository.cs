using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Repositories
{
    public class HorseRepository : IHorseRepository
    {
        private readonly IDbContextFactory<HorsesContext> _contextFactory;
        public HorseRepository(IDbContextFactory<HorsesContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Horse> Create(Horse horse)
        {
            HorsesContext context = _contextFactory.CreateDbContext();
            await context.Horses.AddAsync(horse);
            await context.SaveChangesAsync();
            return horse;
        }
        public async Task Delete(int id)
        {
            HorsesContext context = _contextFactory.CreateDbContext();
            Horse horseToDelete = await context.Horses.FindAsync(id);
            context.Horses.Remove(horseToDelete);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Horse>> GetAll()
        {
            HorsesContext context = _contextFactory.CreateDbContext();
            return await context.Horses.ToListAsync();
        }
        public async Task<Horse> Get(int id)
        {
            HorsesContext context = _contextFactory.CreateDbContext();
            return await context.Horses.FindAsync(id);
        }

        public async Task<IEnumerable<Horse>> GetByAvailability(bool isAvailable)
        {
            HorsesContext context = _contextFactory.CreateDbContext();
            return await context.Horses.Where(horse => horse.IsAvailable == isAvailable).ToListAsync();
        }

        public async Task Update(Horse horse)
        {
            HorsesContext context = _contextFactory.CreateDbContext();
            Horse horseToUpdate = await context.Horses.FindAsync(horse.HorseId);
            if (horseToUpdate != null)
            {
                horseToUpdate.HorseName = horse.HorseName;
                horseToUpdate.IsAvailable = horse.IsAvailable;
            }
            await context.SaveChangesAsync();
        }
    }
}
