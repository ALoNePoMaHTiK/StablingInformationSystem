using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingTypesController : ControllerBase
    {
        private readonly ITrainingTypeRepository _trainingTypeRepository;
        public TrainingTypesController(ITrainingTypeRepository trainingTypeRepository)
        {
            _trainingTypeRepository = trainingTypeRepository;
        }

        /// <summary>
        ///     Получение списка всех типов тренировок
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<TrainingType>> Get()
        {
            return await _trainingTypeRepository.Get();
        }

        /// <summary>
        ///     Получение типа тренировки по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingType>> Get(int id)
        {
            return await _trainingTypeRepository.Get(id);
        }

        /// <summary>
        ///     Добавление типа тренировки
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TrainingType>> Post([FromBody] TrainingType type)
        {
            TrainingType newType = await _trainingTypeRepository.Create(type);
            return CreatedAtAction(nameof(Get), new { id = newType.TrainingTypeId }, newType);
        }

        /// <summary>
        ///     Изменение типа тренировки
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<TrainingType>> Put([FromBody] TrainingType type)
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
