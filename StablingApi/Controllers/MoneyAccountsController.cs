using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoneyAccountsController : ControllerBase
    {
        private readonly IMoneyAccountRepository _repository;

        public MoneyAccountsController(IMoneyAccountRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Получение списка всех денежных счетов
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<MoneyAccount>> GetAll()
        {
            return await _repository.GetAll();
        }

        /// <summary>
        ///     Получение денежного счета по первичному ключу
        /// </summary>
        [HttpGet("{id}")]
        public async Task<MoneyAccount> Get(byte id)
        {
            return await _repository.Get(id);
        }
    }
}