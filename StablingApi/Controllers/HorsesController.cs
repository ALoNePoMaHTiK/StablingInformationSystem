using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace Stabling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorsesController : ControllerBase
    {
        private readonly IHorseRepository _horseRepository;

        public HorsesController(IHorseRepository horseRepository)
        {
            _horseRepository = horseRepository;
        }

        /// <summary>
        ///     Получение списка всех лошадей
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Horse>> GetAll()
        {
            return await _horseRepository.GetAll();
        }

        /// <summary>
        ///     Получение лошади по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Horse>> Get(int id)
        {
            return await _horseRepository.Get(id);
        }

        /// <summary>
        ///     Добавление лошади
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Horse>> Create([FromBody] Horse horse)
        {
            Horse newHorse = await _horseRepository.Create(horse);
            return CreatedAtAction(nameof(Get), new { id = newHorse.HorseId }, newHorse);
        }

        /// <summary>
        ///     Изменение лошади
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<Horse>> Update([FromBody] Horse horse)
        {
            if (_horseRepository.Get(horse.HorseId) == null)
            {
                return NotFound();
            }
            await _horseRepository.Update(horse);
            return NoContent();
        }

        /// <summary>
        ///     Удаление лошади по идентификатору
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Horse horseToDelete = await _horseRepository.Get(id);
            if (horseToDelete == null)
                return NotFound();
            await _horseRepository.Delete(id);
            return NoContent();
        }
    }
}
