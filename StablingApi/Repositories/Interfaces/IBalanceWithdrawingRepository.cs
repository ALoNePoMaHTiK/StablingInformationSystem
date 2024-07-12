using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IBalanceWithdrawingRepository
    {
        Task<BalanceWithdrawing> Get(Guid id);
        Task<BalanceWithdrawingForShow> GetForShow(Guid id);
        Task<IEnumerable<BalanceWithdrawingForShow>> GetForShowByDate(DateTime date);
        Task<IEnumerable<BalanceWithdrawingForShow>> GetForShowByTraining(int trainingId);
        Task<BalanceWithdrawing> Create(BalanceWithdrawing withdrawing);
        Task<BalanceWithdrawing> CreateByTraining(BalanceWithdrawing withdrawing, int trainingId);
        Task<BalanceWithdrawing> CreateByAbonement(BalanceWithdrawing withdrawing, int abonementId);
        Task Update(BalanceWithdrawing withdrawing);
        Task Delete(Guid id);
    }
}