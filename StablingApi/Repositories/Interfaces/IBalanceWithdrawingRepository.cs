using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IBalanceWithdrawingRepository
    {
        Task<IEnumerable<BalanceWithdrawing>> GetAll();
        Task<BalanceWithdrawing> Get(Guid id);
        Task<IEnumerable<BalanceWithdrawing>> GetByClient(int clientId);
        Task<IEnumerable<BalanceWithdrawing>> GetByDate(DateTime date);
        Task<TrainingWithdrawing> GetTrainingCause(Guid withdrawingId);
        Task<AbonementWithdrawing> GetAbonementCause(Guid withdrawingId);
        Task<BalanceWithdrawing> Create(BalanceWithdrawing withdrawing, TrainingWithdrawing trainingWithdrawing);
        Task<BalanceWithdrawing> Create(BalanceWithdrawing withdrawing, AbonementWithdrawing abonementWithdrawing);
        Task Update(BalanceWithdrawing withdrawing);
        Task Delete(int id);
    }
}