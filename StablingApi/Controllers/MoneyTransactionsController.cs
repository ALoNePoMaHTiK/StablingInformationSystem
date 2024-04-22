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
        ///     Получение транзакции по первичному ключу
        /// </summary>
        [HttpGet("{id}")]
        public async Task<MoneyTransaction> Get(int id)
        {
            return await _repository.Get(id);
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
        public async Task<ActionResult> Create([FromBody] MoneyTransaction transaction)
        {
            MoneyTransaction newTransaction = await _repository.Create(transaction);
            return CreatedAtAction(nameof(Create), newTransaction);
        }
    }
}
