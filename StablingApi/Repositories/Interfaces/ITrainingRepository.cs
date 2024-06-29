using StablingApi.Models;

namespace StablingApi.Repositories.Interfaces
{
    public interface ITrainingRepository
    {
        Task<IEnumerable<Training>> GetAll();
        Task<Training> Get(int id);
        Task<TrainingForShow> GetForShow(int id);
        Task<IEnumerable<Training>> GetByWeek(DateTime dateTime);
        Task<IEnumerable<Training>> GetByDay(DateTime dateTime);
        Task<IEnumerable<Training>> GetAllNotPaid();
        Task<IEnumerable<TrainingForShow>> GetAllNotPaidForShow();
        Task<IEnumerable<Training>> Get(DateTime dateTime, int horseId);
        Task<IEnumerable<TrainingForShow>> GetAllForShow();
        Task<IEnumerable<TrainingForShow>> GetForShowByDay(DateTime dateTime);
        Task<Training> Create(Training training);
        Task Update(Training training);
        Task ChangePaidStatus(int trainingId);
        Task Transfer(int id, DateTime dateTime);
        Task Delete(int id);
    }
}