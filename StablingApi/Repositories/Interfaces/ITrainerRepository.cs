using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface ITrainerRepository
    {
        Task<IEnumerable<Trainer>> GetAll();
        Task<Trainer> Get(int id);
        Task<IEnumerable<Trainer>> GetByAvailability(bool isAvailable);
    }
}
