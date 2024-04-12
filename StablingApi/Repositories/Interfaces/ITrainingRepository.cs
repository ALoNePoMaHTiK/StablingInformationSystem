using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface ITrainingRepository
    {
        Task<IEnumerable<Training>> GetAll();
        Task<Training> Get(int id);
        Task<IEnumerable<Training>> GetByWeek(DateTime dateTime);
        Task<IEnumerable<Training>> GetByDay(DateTime dateTime);
        Task<IEnumerable<Training>> GetAllNotPaid();
        Task<IEnumerable<Training>> Get(DateTime dateTime, int horseId);
        Task<Training> Create(Training training);
        Task Update(Training training);
        Task Delete(int id);
    }
}