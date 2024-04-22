using StablingApi.Models;
namespace StablingApi.Repositories.Interfaces
{
    public interface IMoneyTransactionRepository
    {
        Task<IEnumerable<MoneyTransaction>> GetAll();
        Task<MoneyTransaction> Get(int id);
        Task<IEnumerable<MoneyTransaction>> GetByDay(System.DateTime date);
        Task<IEnumerable<MoneyTransaction>> GetByTrainer(int id);
        Task<MoneyTransaction> Create(MoneyTransaction transaction);
        Task Update(MoneyTransaction transaction);
    }
}