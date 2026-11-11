using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    /// <summary>
    /// Контроллер типов техники
    /// </summary>
    /// <param name="types"></param>
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicTypeController(IRepository<TechnicType, int> types) : ControllerBase
    {
        /// <summary>Список всех типов</summary>
        [HttpGet]
        public Task<List<TechnicType>> GetAll(CancellationToken ct) => types.GetAllAsync(ct);
    }
}
