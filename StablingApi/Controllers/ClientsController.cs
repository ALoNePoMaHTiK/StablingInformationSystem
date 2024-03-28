using StablingApi.Models;
using StablingApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace StablingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _repository;

        public ClientsController(IClientRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Получение списка всех клиентов
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _repository.GetAll();
        }

        /// <summary>
        ///     Получение клиента по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> Get(int id)
        {
            return await _repository.Get(id);
        }

        /// <summary>
        ///     Получение клиента по идентификатору
        /// </summary>
        [HttpGet("ByAvailable")]
        public async Task<IEnumerable<Client>> GetByAvailability([FromQuery] bool IsAvailable)
        {
            return await _repository.GetByAvailability(IsAvailable);
        }

        /// <summary>
        ///     Получение списка клиента по идентификатору тренера
        /// </summary>
        [HttpGet("ByTrainer/{id}")]
        public async Task<IEnumerable<Client>> GetByTrainerId(int id)
        {
            return await _repository.GetByTrainer(id);
        }

        /// <summary>
        ///     Добавление клиента
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            Client newClient = await _repository.Create(client);
            return Ok();
        }

        /// <summary>
        ///     Изменение клиента
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<Client>> UpdateClient([FromBody] Client client)
        {
            if (_repository.Get(client.ClientId) == null)
            {
                return NotFound();
            }
            await _repository.Update(client);
            return Ok();
        }

        /// <summary>
        ///     Удаление клиента
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClient(int id)
        {
            Client clientToDelete = await _repository.Get(id);
            if (clientToDelete == null)
                return NotFound();
            await _repository.Delete(id);
            return NoContent();
        }
    }
}