using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessOperationsController : ControllerBase
    {
        private readonly IBusinessOperationRepository _repository;
        public BusinessOperationsController(IBusinessOperationRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Получение списка всех бизнес-операций
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<BusinessOperation>> GetAll()
        {
            return await _repository.GetAll();
        }

        /// <summary>
        ///     Получение бизнес-операции по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessOperation>> Get(int id)
        {
            return await _repository.Get(id);
        }

        /// <summary>
        ///     Получение бизнес-операции для отображения по идентификатору
        /// </summary>
        [HttpGet("ForShow/{id}")]
        public async Task<ActionResult<BusinessOperationForShow>> GetForShow(int id)
        {
            return await _repository.GetForShow(id);
        }

        /// <summary>
        ///     Получение списка доходных бизнес-операций за конкретные сутки
        /// </summary>
        [HttpGet("ByIncome/{date}")]
        public async Task<ActionResult<IEnumerable<BusinessOperationForShow>>> GetByIncome(DateTime date)
        {
            return Ok(await _repository.GetByIncome(date));
        }

        /// <summary>
        ///     Получение списка расходных бизнес-операций за конкретные сутки
        /// </summary>
        [HttpGet("ByConsumption/{date}")]
        public async Task<ActionResult<IEnumerable<BusinessOperationForShow>>> GetByConsumption(DateTime date)
        {
            return Ok(await _repository.GetByConsumption(date));
        }

        /// <summary>
        ///     Добавление бизнес-операции
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Create([FromBody] BusinessOperation operation)
        {
            BusinessOperation newOperation = await _repository.Create(operation);
            return CreatedAtAction(nameof(Create), newOperation);
        }

        /// <summary>
        ///     Изменение бизнес-операции
        /// </summary>
        /// <response code="200" nullable="true"></response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] BusinessOperation operation)
        {
            if (_repository.Get(operation.BusinessOperationId) == null)
            {
                return NotFound();
            }
            await _repository.Update(operation);
            return Ok(operation);
        }
    }
}
