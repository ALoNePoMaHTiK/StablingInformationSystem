using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IBalanceReplenishmentRepository
    {
        Task<IEnumerable<BalanceReplenishment>> GetAll();
        Task<BalanceReplenishment> Get(int id);
        Task<IEnumerable<BalanceReplenishment>> GetByDate(DateTime date);
        Task<BalanceReplenishment> Create(BalanceReplenishment replenishment);
        Task Update(BalanceReplenishment replenishment);
        Task Delete(int id);
    }
}