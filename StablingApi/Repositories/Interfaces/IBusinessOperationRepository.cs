using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IBusinessOperationRepository
    {
        Task<IEnumerable<BusinessOperation>> GetAll();
        Task<BusinessOperation> Get(int id);
        Task<BusinessOperationForShow> GetForShow(int id);
        Task<IEnumerable<BusinessOperation>> GetByDay(DateTime dateTime);
        Task<IEnumerable<BusinessOperationForShow>> GetByIncome(DateTime dateTime);
        Task<IEnumerable<BusinessOperationForShow>> GetByConsumption(DateTime dateTime);
        Task<BusinessOperation> Create(BusinessOperation operation);
        Task Update(BusinessOperation operation);
        Task Delete(int id);
    }
}