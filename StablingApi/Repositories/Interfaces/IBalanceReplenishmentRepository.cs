using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IBalanceReplenishmentRepository
    {
        Task<IEnumerable<BalanceReplenishment>> GetAll();
        Task<BalanceReplenishment> Get(Guid id);
        Task<BalanceReplenishmentForShow> GetForShow(Guid id);
        Task<IEnumerable<BalanceReplenishment>> GetByDate(DateTime date);
        Task<IEnumerable<BalanceReplenishmentForShow>> GetForShowByDate(DateTime date);
        Task<BalanceReplenishment> Create(BalanceReplenishment replenishment);
        Task Update(BalanceReplenishment replenishment);
        Task Delete(Guid id);
    }
}