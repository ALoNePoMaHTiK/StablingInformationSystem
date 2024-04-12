using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;
namespace StablingApi.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly IDbContextFactory<TrainingsContext> _contextFactory;
        public TrainingRepository(IDbContextFactory<TrainingsContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Training> Create(Training training)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            await context.AddAsync(training);
            await context.SaveChangesAsync();
            return training;
        }
        public async Task Delete(int id)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            Training trainingToDelete = await context.Trainings.FindAsync(id);
            context.Trainings.Remove(trainingToDelete);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Training>> GetAll()
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.Trainings.ToListAsync();
        }

        public async Task<Training> Get(int id)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.Trainings.FindAsync(id);
        }

        public async Task<IEnumerable<Training>> Get(DateTime dateTime, int horseId)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.Trainings.Where
                (training => training.TrainingDateTime.Date == dateTime.Date &&
                training.HorseId == horseId).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetByDay(DateTime dateTime)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.Trainings.Where
                (training => training.TrainingDateTime.Date == dateTime).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetByWeek(DateTime dateTime)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.Trainings.Where
                (training => training.TrainingDateTime >= dateTime &&
                training.TrainingDateTime < dateTime.AddDays(7)).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetAllNotPaid()
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.Trainings.Where(t => context.TrainingWithdrawings.Where(w => w.TrainingId == t.TrainingId).FirstOrDefault() == null).ToListAsync();
        }

        public async Task Update(Training training)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            Training trainingToUpdate = await context.Trainings.SingleOrDefaultAsync(t => t.TrainingId == training.TrainingId);
            if (trainingToUpdate != null)
            {
                trainingToUpdate.TrainingTypeId = training.TrainingTypeId;
                trainingToUpdate.TrainerId = training.TrainerId;
                trainingToUpdate.HorseId = training.HorseId;
                trainingToUpdate.ClientId = training.ClientId;
                trainingToUpdate.TrainingDateTime = training.TrainingDateTime;
                context.SaveChanges();
            }
        }
    }
}