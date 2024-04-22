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
        ///     Получение списка тренировок для отображения списка в клиентском приложении
        /// </summary>
        [HttpGet("ForShow/ByDate/{dateTime}")]
        public async Task<IEnumerable<TrainingForShow>> GetForShowByDate(DateTime dateTime)
        {
            return await _repository.GetForShowByDay(dateTime);
        }

        /// <summary>
        ///     Добавление тренировки
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Training>> Create([FromBody] Training training)
        {
            Training newTraining = await _repository.Create(training);
            return CreatedAtAction(nameof(Create), training);
        }

        /// <summary>
        ///     Изменение тренировки
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Training>> Update([FromBody] Training training)
        {
            if (_repository.Get(training.TrainingId) == null)
            {
                return NotFound();
            }
            await _repository.Update(training);
            return Ok(training);
        }

        /// <summary>
        ///     Перенос тренировки на новую дату+время
        /// </summary>
        /// <param name="id">Идентификатор тренировки</param>
        /// <param name="dateTime">Новые дата+время</param>
        [HttpPut("Transfer/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Training>> Transfer(int id,[FromBody] DateTime dateTime)
        {
            if (_repository.Get(id) == null)
            {
                return NotFound();
            }
            await _repository.Transfer(id,dateTime);
            return Ok(id);
        }

        /// <summary>
        ///     Удаление тренировки
        /// </summary>
        /// <param name="id">Идентификатор тренировки</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(int id)
        {
            Training trainingToDelete = await _repository.Get(id);
            if (trainingToDelete == null)
                return NotFound();
            await _repository.Delete(id);
            return Ok(id);
        }
    }
}
