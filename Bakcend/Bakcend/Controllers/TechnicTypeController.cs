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
    public class TechnicTypeController(ITechnicTypeService service, ILogger<TechnicTypeController> logger) : ControllerBase
    {
        /// <summary>Список всех типов</summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TechnicType>>> GetAll(CancellationToken ct)
        {
            var items = await service.GetAll(ct);
            logger.LogInformation("Успешное получение списка типов техники");
            return items is { Count: > 0 } ? Ok(items) : NotFound(new { title = "Типы техники не найдены" });
        }

        /// <summary>Добавление типов техники</summary>
        [HttpPost]
        public async Task<ActionResult<Technic>> Create(TechnicType item, CancellationToken ct)
        {
            try
            {
                var created = await service.Add(item, ct);
                logger.LogInformation("Успешное добавление типа техники");
                return Ok(created);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при добавлении типа техники");
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
                    var message = "ID не совпадает";
                    logger.LogWarning(message);
                    return BadRequest(new { title = message });
                }

                await service.Update(item, ct);
                logger.LogInformation("Успешное обновление типа техники");
                return Ok(item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при обновлении типа техники");
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
                logger.LogInformation("Успешное удаление типа техники");
                return deleted ? Ok(true) : NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при удалении типа техники");
                return BadRequest(new { title = ex.Message });
            }
        }
    }
}
