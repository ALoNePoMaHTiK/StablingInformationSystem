using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbonementsController : Controller
    {
        private readonly IAbonementRepository _repository;
        private readonly IAbonementTypeRepository _abonementTypeRepository;
        public AbonementsController(IAbonementRepository repository, IAbonementTypeRepository abonementTypeRepository)
        {
            _repository = repository;
            _abonementTypeRepository = abonementTypeRepository;
        }

        /// <summary>
        ///     Получение списка абонементов
        /// </summary>
        [HttpGet("ForShow")]
        public async Task<IEnumerable<AbonementForShow>> GetAllForShow()
        {
            return await _repository.GetAllForShow();
        }

        /// <summary>
        ///     Получение абонемента по идентификатору
        /// </summary>
        [HttpGet("{abonementId}")]
        public async Task<Abonement> Get(int abonementId)
        {
            return await _repository.Get(abonementId);
        }

        /// <summary>
        ///     Получение абонемента по идентификатору
        /// </summary>
        [HttpGet("ForShow/{abonementId}")]
        public async Task<AbonementForShow> GetForShow(int abonementId)
        {
            return await _repository.GetForShow(abonementId);
        }

        /// <summary>
        ///     Получение абонемента по идентификатору
        /// </summary>
        [HttpGet("ByAvailability/ForShow/{isAvailable}")]
        public async Task<IEnumerable<AbonementForShow>> GetForShowByAvailability(bool isAvailable)
        {
            return await _repository.GetForShowByAvailability(isAvailable);
        }

        /// <summary>
        ///     Добавление абонемента
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Abonement>> Create([FromBody] Abonement abonement)
        {
            Abonement newAbonement = await _repository.Create(abonement);
            return CreatedAtAction(nameof(Create), abonement);
        }

        /// <summary>
        ///     Изменение тренировки
        /// </summary>
        [HttpPut("ChangeAvailability/{abonementId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> ChangeAvailability(int abonementId)
        {
            if (_repository.Get(abonementId) == null)
            {
                return NotFound();
            }
            await _repository.ChangeAvailability(abonementId);
            return Ok(abonementId);
        }

        /// <summary>
        ///     Получение списка типов абонементов
        /// </summary>
        [HttpGet("Types")]
        public async Task<IEnumerable<AbonementType>> GetTypes()
        {
            return await _abonementTypeRepository.GetAll();
        }

        /// <summary>
        ///     Удаление абонемента
        /// </summary>
        /// <param name="id">Идентификатор абонемента</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(int id)
        {
            Abonement abonementToDelete = await _repository.Get(id);
            if (abonementToDelete == null)
                return NotFound();
            await _repository.Delete(id);
            return Ok(id);
        }
    }
}
