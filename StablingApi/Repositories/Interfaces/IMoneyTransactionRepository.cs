using StablingApi.Models;
namespace StablingApi.Repositories.Interfaces
{
    public interface IMoneyTransactionRepository
    {
        Task<IEnumerable<MoneyTransaction>> GetAll();
        Task<IEnumerable<MoneyTransactionForShow>> GetForShowByDate(DateTime date);
        Task<MoneyTransactionForShow> GetForShow(int id);
        Task<MoneyTransaction> Get(int id);
        Task<IEnumerable<MoneyTransaction>> GetByDay(DateTime date);
        Task<IEnumerable<MoneyTransaction>> GetByTrainer(int id);
        Task<MoneyTransaction> Create(MoneyTransaction transaction);
        Task Update(MoneyTransaction transaction);
        Task Delete(int id);
    }
}