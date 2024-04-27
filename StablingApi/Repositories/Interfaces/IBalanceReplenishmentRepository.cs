using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IBalanceReplenishmentRepository
    {
        Task<IEnumerable<BalanceReplenishment>> GetAll();
        Task<BalanceReplenishment> Get(int id);
        Task<BalanceReplenishmentForShow> GetForShow(int id);
        Task<IEnumerable<BalanceReplenishment>> GetByDate(DateTime date);
        Task<IEnumerable<BalanceReplenishmentForShow>> GetByDateForShow(DateTime date);
        Task<BalanceReplenishment> Create(BalanceReplenishment replenishment);
        Task Update(BalanceReplenishment replenishment);
        Task Delete(int id);
    }
}