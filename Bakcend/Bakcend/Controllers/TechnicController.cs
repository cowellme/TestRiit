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
    public class TechnicController(ITechnicService service, ILogger<TechnicController> logger) : ControllerBase
    {
        /// <summary>Список всей техники</summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Technic>>> GetAll(CancellationToken ct)
        {
            var items = await service.GetAll(ct);
            logger.LogInformation("Успешное получение списка техники");
            return items is { Count: > 0 } ? Ok(items) : NotFound(new { title = "Техника не найдена" });
        }

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
                logger.LogError(ex, "Ошибка при получении техники");
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
                const string message = "Учётный номер уже существует";
                logger.LogWarning(message);
                return Conflict(new { title = message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при добавлении техники");
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
                    const string message = "ID не совпадает";
                    logger.LogWarning(message);
                    return BadRequest(new { title = message });
                }

                await service.Update(item, ct);
                logger.LogInformation("Успешное обновление техники");
                return Ok(item);
            }
            catch (Exception ex)
            {   
                logger.LogError(ex, "Ошибка при обновлении техники");
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
                logger.LogInformation("Успешное удаление техники");
                return deleted ? Ok(true) : NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при удалении техники");
                return BadRequest(new { title = ex.Message });
            }
        }
    }
}
