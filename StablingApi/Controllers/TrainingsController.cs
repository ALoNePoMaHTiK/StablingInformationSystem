using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly ITrainingRepository _repository;
        private readonly ITrainingTypeRepository _trainingTypeRepository;

        public TrainingsController(ITrainingRepository repository, ITrainingTypeRepository trainingTypeRepository)
        {
            _repository = repository;
            _trainingTypeRepository = trainingTypeRepository;
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
        ///     Получение представления тренировки по идентификатору
        /// </summary>
        [HttpGet("ForShow/{id}")]
        public async Task<ActionResult<TrainingForShow>> GetForShow(int id)
        {
            var training = await _repository.GetForShow(id);
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
        ///     Получение списка представлений неоплаченных тренировок
        /// </summary>
        [HttpGet("Paid/ForShow/")]
        public async Task<IEnumerable<TrainingForShow>> GetAllNotPaidForShow()
        {
            return await _repository.GetAllNotPaidForShow();
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
        ///     Изменение статуса оплаты тренировки
        /// </summary>
        [HttpPut("PaidStatus/{trainingId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> ChangePaidStatus(int trainingId)
        {
            await _repository.ChangePaidStatus(trainingId);
            return Ok(trainingId);
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

        /// <summary>
        ///     Получение списка всех типов тренировок
        /// </summary>
        [HttpGet("Types")]
        public async Task<IEnumerable<TrainingType>> GetTypes()
        {
            return await _trainingTypeRepository.GetAll();
        }

        /// <summary>
        ///     Получение списка всех типов тренировок
        /// </summary>
        [HttpGet("Types/{typeId}")]
        public async Task<TrainingType> GetType(int typeId)
        {
            return await _trainingTypeRepository.Get(typeId);
        }

        /// <summary>
        ///     Добавление типа тренировки
        /// </summary>
        [HttpPost("Types")]
        public async Task<ActionResult<TrainingType>> Create([FromBody] TrainingType type)
        {
            TrainingType newType = await _trainingTypeRepository.Create(type);
            return CreatedAtAction(nameof(Create), newType);
        }

        /// <summary>
        ///     Изменение типа тренировки
        /// </summary>
        [HttpPut("Types")]
        public async Task<ActionResult<TrainingType>> Update([FromBody] TrainingType type)
        {
            if (_trainingTypeRepository.Get(type.TrainingTypeId) == null)
            {
                return NotFound();
            }
            await _trainingTypeRepository.Update(type);
            return Ok();
        }
    }
}
