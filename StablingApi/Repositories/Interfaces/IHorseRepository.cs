using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IHorseRepository
    {
        Task<IEnumerable<Horse>> GetAll();
        Task<Horse> Get(int id);
        Task<IEnumerable<Horse>> GetByAvailability(bool isAvailable);
        Task<Horse> Create(Horse horse);
        Task Update(Horse horse);
        Task Delete(int id);
    }
}