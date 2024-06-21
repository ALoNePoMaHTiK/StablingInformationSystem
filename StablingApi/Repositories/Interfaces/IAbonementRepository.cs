using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IAbonementRepository
    {
        Task<IEnumerable<Abonement>> GetAll();
        Task<IEnumerable<AbonementForShow>> GetAllForShow();
        Task<Abonement> Get(int id);
        Task<IEnumerable<Abonement>> GetByClient(int id);
        Task<IEnumerable<Abonement>> GetByType(int id);
        Task<IEnumerable<Abonement>> Get(bool IsAvailable);
        Task<IEnumerable<Abonement>> GetAllNotPaid();
        Task<Abonement> Create(Abonement abonement);
        Task Update(Abonement abonement);
        Task Delete(int id);
    }
}