using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IAbonementRepository
    {
        Task<IEnumerable<Abonement>> GetAll();
        Task<IEnumerable<AbonementForShow>> GetAllForShow();
        Task<IEnumerable<AbonementForShow>> GetForShowByAvailability(bool isAvailable);
        Task<Abonement> Get(int id);
        Task<AbonementForShow> GetForShow(int id);
        Task<IEnumerable<Abonement>> GetByClient(int id);
        Task<IEnumerable<Abonement>> GetByType(int id);
        Task<IEnumerable<Abonement>> Get(bool IsAvailable);
        Task<IEnumerable<Abonement>> GetAllNotPaid();
        Task<Abonement> Create(Abonement abonement);
        Task Update(Abonement abonement);
        Task ChangeAvailability(int id);
        Task Delete(int id);
    }
}