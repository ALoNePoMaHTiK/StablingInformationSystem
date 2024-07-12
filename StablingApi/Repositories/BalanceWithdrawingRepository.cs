using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Repositories
{
    public class BalanceWithdrawingRepository : IBalanceWithdrawingRepository
    {
        private readonly IDbContextFactory<MoneyContext> _contextFactory;
        public BalanceWithdrawingRepository(IDbContextFactory<MoneyContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<BalanceWithdrawing> Create(BalanceWithdrawing withdrawing)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            await context.BalanceWithdrawings.AddAsync(withdrawing);
            return withdrawing;
        }

        public Task<BalanceWithdrawing> CreateByAbonement(BalanceWithdrawing withdrawing, int abonementId)
        {
            throw new NotImplementedException();
        }

        public async Task<BalanceWithdrawing> CreateByTraining(BalanceWithdrawing withdrawing, int trainingId)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            await context.BalanceWithdrawings.AddAsync(withdrawing);
            TrainingWithdrawing trainingWithdrawing = new TrainingWithdrawing(trainingId,withdrawing.BalanceWithdrawingId);
            await context.TrainingWithdrawings.AddAsync(trainingWithdrawing);
            await context.SaveChangesAsync();
            return withdrawing;
        }

        public async Task Delete(Guid id)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            BalanceWithdrawing withdrawing = await context.BalanceWithdrawings.FindAsync(id);
            if (withdrawing.WithdrawingCause == "Training")
            {
                context.TrainingWithdrawings.Remove(await context.TrainingWithdrawings.Where(tw =>
                tw.BalanceWithdrawingId == withdrawing.BalanceWithdrawingId).FirstAsync());
                await context.SaveChangesAsync();
            }
            context.BalanceWithdrawings.Remove(withdrawing);
            await context.SaveChangesAsync();
        }

        public async Task<BalanceWithdrawing> Get(Guid id)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.BalanceWithdrawings.FindAsync(id);
        }

        public async Task<BalanceWithdrawingForShow> GetForShow(Guid id)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.BalanceWithdrawingsForShow.Where(bw => bw.BalanceWithdrawingId == id).FirstAsync();
        }

        public async Task<IEnumerable<BalanceWithdrawingForShow>> GetForShowByDate(DateTime date)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.BalanceWithdrawingsForShow.Where(bw => bw.WithdrawingDate == date).ToListAsync();
        }
        
        public async Task<IEnumerable<BalanceWithdrawingForShow>> GetForShowByTraining(int trainingId)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            IEnumerable<Guid> trainingWithdrawings = await context.TrainingWithdrawings.Where(tw => tw.TrainingId == trainingId).Select(tw => tw.BalanceWithdrawingId).ToListAsync();
            return await context.BalanceWithdrawingsForShow.Where(bw => trainingWithdrawings.Contains(bw.BalanceWithdrawingId)).ToListAsync();
        }

        public async Task Update(BalanceWithdrawing withdrawing)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            BalanceWithdrawing withdrawingToUpdate = await context.BalanceWithdrawings.FindAsync(withdrawing.BalanceWithdrawingId);
            if (withdrawingToUpdate != null)
            {
                withdrawingToUpdate.WithdrawingDate = withdrawing.WithdrawingDate;
                withdrawingToUpdate.ClientId = withdrawing.ClientId;
                withdrawingToUpdate.TrainerId = withdrawing.TrainerId;
                withdrawingToUpdate.Amount = withdrawing.Amount;
                withdrawingToUpdate.WithdrawingCause = withdrawing.WithdrawingCause;
            }
            await context.SaveChangesAsync();
        }
    }
}
