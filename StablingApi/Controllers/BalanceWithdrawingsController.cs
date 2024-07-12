using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BalanceWithdrawingsController : ControllerBase
    {
        private readonly IBalanceWithdrawingRepository _repository;
        public BalanceWithdrawingsController(IBalanceWithdrawingRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Получение списания с баланса клиентов по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        public async Task<BalanceWithdrawing> Get(Guid id)
        {
            return await _repository.Get(id);
        }

        /// <summary>
        ///     Получение представления списания с баланса клиента по идентификатору
        /// </summary>
        [HttpGet("ForShow/{id}")]
        public async Task<BalanceWithdrawingForShow> GetForShow(Guid id)
        {
            return await _repository.GetForShow(id);
        }

        /// <summary>
        ///     Получение списка представлений списаний с баланса клиентов на определенную дату
        /// </summary>
        [HttpGet("ForShow/ByDate/{date}")]
        public async Task<IEnumerable<BalanceWithdrawingForShow>> GetForShowByDate(DateTime date)
        {
            return await _repository.GetForShowByDate(date);
        }
        /// <summary>
        ///     Получение списка представлений списаний с баланса клиентов по идентификатору тренировки
        /// </summary>
        [HttpGet("ForShow/ByTraining/{trainingId}")]
        public async Task<IEnumerable<BalanceWithdrawingForShow>> GetForShowByTraining(int trainingId)
        {
            return await _repository.GetForShowByTraining(trainingId);
        }

        /// <summary>
        ///     Добавление списания с баланса клиента
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Create([FromBody] BalanceWithdrawing withdrawing)
        {
            BalanceWithdrawing newWithdrawing = await _repository.Create(withdrawing);
            return CreatedAtAction(nameof(Create), newWithdrawing);
        }

        /// <summary>
        ///     Добавление списания с баланса клиента на основании тренировки
        /// </summary>
        [HttpPost("ByTraining/{trainingId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateByTraining([FromBody] BalanceWithdrawing withdrawing, int trainingId)
        {
            BalanceWithdrawing newWithdrawing = await _repository.CreateByTraining(withdrawing, trainingId);
            return CreatedAtAction(nameof(Create), newWithdrawing);
        }

        /// <summary>
        ///     Изменение списания с баланса клиента
        /// </summary>
        /// <response code="200" nullable="true"></response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] BalanceWithdrawing withdrawing)
        {
            if (_repository.Get(withdrawing.BalanceWithdrawingId) == null)
            {
                return NotFound();
            }
            await _repository.Update(withdrawing);
            return Ok(withdrawing);
        }

        /// <summary>
        ///     Удаление списания с баланса клиента
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            BalanceWithdrawing withdrawingToDelete = await _repository.Get(id);
            if (withdrawingToDelete == null)
                return NotFound();
            await _repository.Delete(id);
            return Ok(id);
        }
    }
}
