using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessOperationTypesController : ControllerBase
    {
        private readonly IBusinessOperationTypeRepository _repository;
        public BusinessOperationTypesController(IBusinessOperationTypeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Получение списка всех  типов бизнес-операций
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<BusinessOperationType>> GetAll()
        {
            return await _repository.GetAll();
        }

        /// <summary>
        ///     Получение списка всех доходных типов бизнес-операций
        /// </summary>
        [HttpGet("ByIncome")]
        public async Task<ActionResult<IEnumerable<BusinessOperationType>>> GetIncomeTypes()
        {
            return Ok(await _repository.GetIncomeTypes());
        }

        /// <summary>
        ///     Получение списка всех расходных типов бизнес-операций
        /// </summary>
        [HttpGet("ByConsumption")]
        public async Task<ActionResult<IEnumerable<BusinessOperationType>>> GetConsumptionTypes()
        {
            return Ok(await _repository.GetConsumptionTypes());
        }
    }
}
