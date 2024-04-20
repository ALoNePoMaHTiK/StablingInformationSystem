using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAll();
        Task<Client> Get(int id);
        Task<IEnumerable<Client>> GetByTrainer(int trainerId);
        Task<IEnumerable<Client>> GetByAvailability(bool IsAvailable);
        Task<Client> Create(Client client);
        Task Update(Client client);
        Task ChangeAvailability(int id);
        Task Delete(int id);
    }
}