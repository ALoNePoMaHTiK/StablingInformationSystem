using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;
namespace StablingApi.Repositories
{
    public class TrainingTypeRepository : ITrainingTypeRepository
    {
        private readonly IDbContextFactory<TrainingsContext> _contextFactory;
        public TrainingTypeRepository(IDbContextFactory<TrainingsContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<TrainingType> Create(TrainingType type)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            await context.TrainingTypes.AddAsync(type);
            await context.SaveChangesAsync();
            return type;
        }

        public async Task<IEnumerable<TrainingType>> GetAll()
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.TrainingTypes.ToListAsync();
        }

        public async Task<TrainingType> Get(int id)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.TrainingTypes.FindAsync(id);
        }

        public async Task Update(TrainingType type)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            TrainingType typeToUpdate = await context.TrainingTypes.FindAsync(type.TrainingTypeId);
            if (typeToUpdate != null)
            {
                typeToUpdate.TypeName = type.TypeName;
                typeToUpdate.TypePrice = type.TypePrice;
                typeToUpdate.TypeDuration = type.TypeDuration;
                typeToUpdate.IsAvailable = type.IsAvailable;
            }
            await context.SaveChangesAsync();
        }
    }
}