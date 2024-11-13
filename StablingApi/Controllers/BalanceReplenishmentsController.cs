using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BalanceReplenishmentsController : ControllerBase
    {
        private readonly IBalanceReplenishmentRepository _repository;
        public BalanceReplenishmentsController(IBalanceReplenishmentRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Получение списка пополнений баланса клиентов
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<BalanceReplenishment>> GetAll()
        {
            return await _repository.GetAll();
        }

        /// <summary>
        ///     Получение пополнения баланса клиентов по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        public async Task<BalanceReplenishment> Get(Guid id)
        {
            return await _repository.Get(id);
        }

        /// <summary>
        ///     Получение списка пополнений баланса клиентов на определенную дату
        /// </summary>
        [HttpGet("ByDate/{date}")]
        public async Task<IEnumerable<BalanceReplenishment>> GetByDate(DateTime date)
        {
            return await _repository.GetByDate(date);
        }

        /// <summary>
        ///     Получение списка представлений пополнений баланса клиентов на определенную дату
        /// </summary>
        [HttpGet("ForShow/ByDate/{date}")]
        public async Task<IEnumerable<BalanceReplenishmentForShow>> GetForShowByDate(DateTime date)
        {
            return await _repository.GetForShowByDate(date);
        }

        /// <summary>
        ///     Получение представления пополнения баланса клиента по идентификатору
        /// </summary>
        [HttpGet("ForShow/{id}")]
        public async Task<BalanceReplenishmentForShow> GetForShow(Guid id)
        {
            return await _repository.GetForShow(id);
        }

        /// <summary>
        ///     Добавление пополнения баланса клиента
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Create([FromBody] BalanceReplenishment replenishment)
        {
            BalanceReplenishment newReplenishment = await _repository.Create(replenishment);
            return CreatedAtAction(nameof(Create), newReplenishment);
        }

        /// <summary>
        ///     Изменение пополнения баланса клиента
        /// </summary>
        /// <response code="200" nullable="true"></response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] BalanceReplenishment replenishment)
        {
            if (_repository.Get(replenishment.BalanceReplenishmentId) == null)
            {
                return NotFound();
            }
            await _repository.Update(replenishment);
            return Ok(replenishment);
        }

        /// <summary>
        ///     Удаление пополнения баланса клиента
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            BalanceReplenishment replenishmentToDelete = await _repository.Get(id);
            if (replenishmentToDelete == null)
                return NotFound();
            await _repository.Delete(id);
            return Ok(id);
        }
    }
}
