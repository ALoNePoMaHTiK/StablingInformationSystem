using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IAbonementUsageRepository
    {
        Task<IEnumerable<AbonementUsage>> Get();
        Task<AbonementUsage> Get(int id);
        Task<IEnumerable<AbonementUsage>> GetByAbonement(int abonementId);
        Task<AbonementUsage> GetByTraining(int trainingId);
        Task<AbonementUsage> Create(AbonementUsage usage);
        Task<AbonementUsage> Create(int abonementId, int trainingId);
        Task Update(AbonementUsage usage);
        Task Delete(int id);
    }
}