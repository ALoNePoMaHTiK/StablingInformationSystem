using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface IMoneyAccountRepository
    {
        Task<IEnumerable<MoneyAccount>> GetAll();
        Task<MoneyAccount> Get(byte id);
    }
}