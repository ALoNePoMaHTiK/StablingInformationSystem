using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface ITrainingTypeRepository
    {
        Task<IEnumerable<TrainingType>> GetAll();
        Task<TrainingType> Get(int id);
        Task<TrainingType> Create(TrainingType type);
        Task Update(TrainingType type);
    }
}