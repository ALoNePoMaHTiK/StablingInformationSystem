using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly ITrainingRepository _repository;

        public TrainingsController(ITrainingRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Получение списка всех тренировок
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Training>> GetAll()
        {
            return await _repository.GetAll();
        }

        /// <summary>
        ///     Получение тренировки по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Training>> Get(int id)
        {
            var training = await _repository.Get(id);
            if (training == null)
            {
                return NotFound();
            }
            return Ok(training);
        }

        /// <summary>
        ///     Получение списка тренировок за день по дате/времени
        /// </summary>
        [HttpGet("ByDay/{dateTime:datetime}")]
        public async Task<IEnumerable<Training>> GetByDay(DateTime dateTime)
        {
            return await _repository.GetByDay(dateTime);
        }

        /// <summary>
        ///     Получение списка тренировок за неделю по дате/времени
        /// </summary>
        [HttpGet("ByWeek/{dateTime:datetime}")]
        public async Task<IEnumerable<Training>> GetByWeek(DateTime dateTime)
        {
            return await _repository.GetByWeek(dateTime);
        }

        /// <summary>
        ///     Получение списка неоплаченных тренировок
        /// </summary>
        [HttpGet("Paid/")]
        public async Task<IEnumerable<Training>> GetAllNotPaid()
        {
            return await _repository.GetAllNotPaid();
        }

        /// <summary>
        ///     Получение списка тренировок за неделю по дате/времени
        /// </summary>
        [HttpGet("ByHorseDay/{dateTime:datetime}/{horseId:int}")]
        public async Task<IEnumerable<Training>> GetByHorseDay(DateTime dateTime, int horseId)
        {
            return await _repository.Get(dateTime, horseId);
        }

        /// <summary>
        ///     Добавление тренировки
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Training>> Create([FromBody] Training training)
        {
            Training newTraining = await _repository.Create(training);
            return CreatedAtAction(nameof(Create), training);
        }

        /// <summary>
        ///     Изменение тренировки
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<Training>> Update([FromBody] Training training)
        {
            if (_repository.Get(training.TrainingId) == null)
            {
                return NotFound();
            }
            await _repository.Update(training);
            return Ok();
        }

        /// <summary>
        ///     Удаление тренировки
        /// </summary>
        /// <param name="id">Идентификатор тренировки</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Training trainingToDelete = await _repository.Get(id);
            if (trainingToDelete == null)
                return NotFound();
            await _repository.Delete(id);
            return NoContent();
        }
    }
}
