﻿using StablingApi.Models;
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
        [HttpGet("ByAvailability/{IsAvailable}")]
        public async Task<IEnumerable<Client>> GetByAvailability(bool IsAvailable)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Create([FromBody] Client client)
        {
            Client newClient = await _repository.Create(client);
            return CreatedAtAction(nameof(Create),newClient);
        }

        /// <summary>
        ///     Изменение статуса клиента
        /// </summary>
        /// <response code="200" nullable="true"></response>
        [HttpPut("ChangeAvailability/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ChangeAvailability(int id)
        {
            if (_repository.Get(id) == null)
            {
                return NotFound();
            }
            await _repository.ChangeAvailability(id);
            return Ok(id);
        }

        /// <summary>
        ///     Изменение клиента
        /// </summary>
        /// <response code="200" nullable="true"></response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] Client client)
        {
            if (_repository.Get(client.ClientId) == null)
            {
                return NotFound();
            }
            await _repository.Update(client);
            return Ok(client);
        }

        /// <summary>
        ///     Удаление клиента
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Client clientToDelete = await _repository.Get(id);
            if (clientToDelete == null)
                return NotFound();
            await _repository.Delete(id);
            return Ok();
        }
    }
}