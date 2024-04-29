using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Repositories.Interfaces;
using StablingApi.Models;

namespace StablingApi.Repositories
{
    public class MoneyTransactionRepository : IMoneyTransactionRepository
    {
        private readonly IDbContextFactory<MoneyContext> _contextFactory;
        public MoneyTransactionRepository(IDbContextFactory<MoneyContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<MoneyTransaction> Create(MoneyTransaction transaction)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            await context.MoneyTransactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return transaction;
        }

        public async Task<MoneyTransaction> Get(int id)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.MoneyTransactions.FindAsync(id);
        }

        public async Task<MoneyTransactionForShow> GetForShow(int id)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.MoneyTransactionsForShow.Where(
                transaction => transaction.MoneyTransactionId == id).FirstAsync();
        }

        public async Task<IEnumerable<MoneyTransaction>> GetAll()
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.MoneyTransactions.ToListAsync();
        }
        
        public async Task<IEnumerable<MoneyTransactionForShow>> GetForShowByDate(DateTime date)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.MoneyTransactionsForShow.Where(
                transaction => transaction.TransactionDate.Date == date.Date).ToListAsync();
        }

        public async Task<IEnumerable<MoneyTransaction>> GetByDay(DateTime date)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            return await context.MoneyTransactions.Where(transaction => transaction.TransactionDate.Date == date.Date).ToListAsync();
        }

        public Task<IEnumerable<MoneyTransaction>> GetByTrainer(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task Update(MoneyTransaction transaction)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            MoneyTransaction transactionToUpdate = await context.MoneyTransactions.FindAsync(transaction.MoneyTransactionId);
            if (transactionToUpdate != null)
            {
                transactionToUpdate.TrainerId = transaction.TrainerId;
                transactionToUpdate.MoneyAccountId = transaction.MoneyAccountId;
                transactionToUpdate.TransactionDate = transaction.TransactionDate;
                transactionToUpdate.Amount = transaction.Amount;
            }
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            MoneyContext context = _contextFactory.CreateDbContext();
            MoneyTransaction transactionForDelete = await context.MoneyTransactions.FindAsync(id);
            context.MoneyTransactions.Remove(transactionForDelete);
            await context.SaveChangesAsync();
        }
    }
}
