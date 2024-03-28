using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IAbonementTypeRepository
    {
        Task<IEnumerable<AbonementType>> GetAll();
        Task<AbonementType> Get(int id);
        Task<AbonementType> Create(AbonementType type);
        Task Update(AbonementType type);
        Task Delete(int id);
    }
}