using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoneyTransactionsController : ControllerBase
    {
        private readonly IMoneyTransactionRepository _repository;

        public MoneyTransactionsController(IMoneyTransactionRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Получение списка всех транзакций
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<MoneyTransaction>> GetAll()
        {
            return await _repository.GetAll();
        }

        /// <summary>
        ///     Получение списка представлений всех транзакций
        /// </summary>
        [HttpGet("ByDate/ForShow/{date}")]
        public async Task<IEnumerable<MoneyTransactionForShow>> GetForShowByDate(DateTime date)
        {
            return await _repository.GetForShowByDate(date);
        }

        /// <summary>
        ///     Получение транзакции по первичному ключу
        /// </summary>
        [HttpGet("{id}")]
        public async Task<MoneyTransaction> Get(int id)
        {
            return await _repository.Get(id);
        }

        /// <summary>
        ///     Получение списка представлений всех транзакций
        /// </summary>
        [HttpGet("ForShow/{id}")]
        public async Task<MoneyTransactionForShow> GetForShow(int id)
        {
            return await _repository.GetForShow(id);
        }

        /// <summary>
        ///     Получение списка транзакций по дате
        /// </summary>
        [HttpGet("ByDate/{date}")]
        public async Task<IEnumerable<MoneyTransaction>> GetByDate(DateTime date)
        {
            return await _repository.GetByDay(date);
        }

        /// <summary>
        ///     Получение списка транзакций по дате
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<MoneyTransaction>> Create([FromBody] MoneyTransaction transaction)
        {
            MoneyTransaction newTransaction = await _repository.Create(transaction);
            return CreatedAtAction(nameof(Create), newTransaction);
        }

        /// <summary>
        ///     Изменение транзакции
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MoneyTransaction>> Update([FromBody] MoneyTransaction transaction)
        {
            if (_repository.Get(transaction.MoneyTransactionId) == null)
            {
                return NotFound();
            }
            await _repository.Update(transaction);
            return Ok(transaction);
        }

        /// <summary>
        ///     Удаление транзакции
        /// </summary>
        /// <param name="id">Идентификатор транзакции</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(int id)
        {
            MoneyTransaction transactionForDelete = await _repository.Get(id);
            if (transactionForDelete == null)
                return NotFound();
            await _repository.Delete(id);
            return Ok(id);
        }
    }
}
