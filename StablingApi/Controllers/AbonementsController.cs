using Microsoft.AspNetCore.Mvc;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbonementsController : Controller
    {
        private readonly IAbonementRepository _repository;
        public AbonementsController(IAbonementRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Получение списка абонементов
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<AbonementForShow>> GetAllForShow()
        {
            return await _repository.GetAllForShow();
        }
    }
}
