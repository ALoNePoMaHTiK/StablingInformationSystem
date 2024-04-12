using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerRepository _repository;

        public TrainersController(ITrainerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Получение списка всех тренировок
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Trainer>> GetAll()
        {
            return await _repository.GetAll();
        }
    }
}
