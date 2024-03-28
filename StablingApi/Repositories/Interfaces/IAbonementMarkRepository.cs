using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IAbonementMarkRepository
    {
        Task<IEnumerable<AbonementMark>> Get();
        Task<AbonementMark> Get(int id);
        Task<IEnumerable<AbonementMark>> GetByAbonement(int abonementId);
        Task<AbonementMark> GetByTraining(int trainingId);
        Task<AbonementMark> Create(AbonementMark mark);
        Task Update(AbonementMark mark);
        Task Delete(int id);
    }
}