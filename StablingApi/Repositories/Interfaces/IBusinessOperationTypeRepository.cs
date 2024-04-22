using StablingApi.Models;
namespace StablingApi.Repositories.Interfaces
{
    public interface IBusinessOperationTypeRepository
    {
        Task<IEnumerable<BusinessOperationType>> GetAll();

        //Получение типа бизнес операции по первичному ключу
        Task<BusinessOperationType> Get(int id);

        Task<IEnumerable<BusinessOperationType>> GetIncomeTypes();
        Task<IEnumerable<BusinessOperationType>> GetConsumptionTypes();

        Task<BusinessOperationType> Create(BusinessOperationType type);
        Task Update(BusinessOperationType type);
    }
}
