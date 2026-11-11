using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    /// <summary>
    /// КОнтроллер для управления списком техники
    /// </summary>
    /// <param name="service"></param>
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicController(ITechnicService service) : ControllerBase
    {
        /// <summary>Список всей техники</summary>
        [HttpGet]
        public Task<IReadOnlyList<Technic>> GetAll(CancellationToken ct) => service.GetAll(ct);

        /// <summary>Получение техники по ID</summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Technic>> Get(int id, CancellationToken ct)
        {
            try
            {
                var item = await service.GetById(id, ct);
                return item is null ? NotFound("Такого ID не существует") : Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(new { title = ex.Message});
            }
        }

        /// <summary>Добавление техники</summary>
        [HttpPost]
        public async Task<ActionResult<Technic>> Create(Technic item, CancellationToken ct)
        {
            try
            {
                var created = await service.Add(item, ct);
                return Ok(created);
            }
            catch (DbUpdateException)
            {
                return Conflict(new { title = "Учётный номер уже существует" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { title = ex.Message });
            }
        }

        /// <summary>Обновление техники</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Technic item, CancellationToken ct)
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

        /// <summary>Удаление техники</summary>
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
