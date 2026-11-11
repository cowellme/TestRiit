using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    /// <summary>
    /// Контроллер типов техники
    /// </summary>
    /// <param name="service"></param>
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicTypeController(ITechnicTypeService service) : ControllerBase
    {
        /// <summary>Список всех типов</summary>
        [HttpGet]
        public async Task<IReadOnlyList<TechnicType>> GetAll(CancellationToken ct) => await service.GetAll(ct);

        /// <summary>Добавление типов техники</summary>
        [HttpPost]
        public async Task<ActionResult<Technic>> Create(TechnicType item, CancellationToken ct)
        {
            try
            {
                var created = await service.Add(item, ct);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { title = ex.Message });
            }
        }

        /// <summary>Обновление типов техники</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TechnicType item, CancellationToken ct)
        {
            try
            {
                if (id != item.Id)
                {
                    return BadRequest(new { title = "ID не совпадает." });
                }

                await service.Update(item, ct);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(new { title = ex.Message });
            }
        }

        /// <summary>Удаление типов техники</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try
            {
                var deleted = await service.Delete(id, ct);
                return deleted ? Ok(true) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { title = ex.Message });
            }
        }
    }
}
