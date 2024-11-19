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
            return await context.Trainings.Where(t => !t.IsPaid).ToListAsync();
        }

        public async Task Update(Training training)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            Training trainingForUpdate = await context.Trainings.SingleOrDefaultAsync(t => t.TrainingId == training.TrainingId);
            if (trainingForUpdate != null)
            {
                trainingForUpdate.TrainingTypeId = training.TrainingTypeId;
                trainingForUpdate.TrainerId = training.TrainerId;
                trainingForUpdate.HorseId = training.HorseId;
                trainingForUpdate.ClientId = training.ClientId;
                trainingForUpdate.TrainingDateTime = training.TrainingDateTime;
                await context.SaveChangesAsync();
            }
        }

        public async Task ChangePaidStatus(int trainingId)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            Training trainingForUpdate = await context.Trainings.SingleOrDefaultAsync(t => t.TrainingId == trainingId);
            if (trainingForUpdate != null)
            {
                trainingForUpdate.IsPaid = !trainingForUpdate.IsPaid;
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TrainingForShow>> GetAllForShow()
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.TrainingsForShow.ToListAsync();
        }

        public async Task<IEnumerable<TrainingForShow>> GetForShowByDay(DateTime dateTime)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.TrainingsForShow.Where
                (training => training.TrainingStart.Date == dateTime.Date).ToListAsync();
        }

        public async Task Transfer(int id, DateTime dateTime)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            Training trainingForUpdate = await context.Trainings.SingleOrDefaultAsync(t => t.TrainingId == id);
            if (trainingForUpdate != null)
            {
                trainingForUpdate.TrainingDateTime = dateTime;
            }
        }

        public async Task<TrainingForShow> GetForShow(int id)
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.TrainingsForShow.Where(tr => tr.TrainingId == id).FirstAsync();
        }

        public async Task<IEnumerable<TrainingForShow>> GetAllNotPaidForShow()
        {
            TrainingsContext context = _contextFactory.CreateDbContext();
            return await context.TrainingsForShow.Where(t => !t.IsPaid).ToListAsync();
        }
    }
}