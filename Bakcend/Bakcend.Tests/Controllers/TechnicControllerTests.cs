using Backend.Controllers;
using Backend.Models;
using Backend.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests.Controllers;

public class TechnicControllerTests
{
    private static Technic NewTechnic() => new()
    {
        AccountNumber = "T001",
        Name = "Тест",
        TechnicTypeId = 1,
        RoomNumber = 101
    };

    [Fact]
    public async Task GetAll_ReturnsAllItems()
    {
        var service = new FakeTechnicService();
        service.All = new List<Technic> { NewTechnic() };
        var controller = new TechnicController(service);

        var result = await controller.GetAll(CancellationToken.None);

        Assert.Single(result);
    }

    [Fact]
    public async Task Get_Existing_ReturnsOk()
    {
        var service = new FakeTechnicService { ById = NewTechnic() };
        var controller = new TechnicController(service);

        var result = await controller.Get(1, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task Get_Missing_ReturnsNotFound()
    {
        var service = new FakeTechnicService { ById = null };
        var controller = new TechnicController(service);

        var result = await controller.Get(999, CancellationToken.None);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_Success_ReturnsOk()
    {
        var service = new FakeTechnicService();
        var controller = new TechnicController(service);

        var result = await controller.Create(NewTechnic(), CancellationToken.None);

        Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(service.Added);
    }

    [Fact]
    public async Task Create_WhenLimitReached_ReturnsBadRequest()
    {
        var service = new FakeTechnicService
        {
            AddException = new InvalidOperationException("Достигнут лимит: не более 2000 записей в БД")
        };
        var controller = new TechnicController(service);

        var result = await controller.Create(NewTechnic(), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_WhenDuplicate_ReturnsConflict()
    {
        var service = new FakeTechnicService
        {
            AddException = new DbUpdateException("duplicate", (Exception?)null)
        };
        var controller = new TechnicController(service);

        var result = await controller.Create(NewTechnic(), CancellationToken.None);

        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_IdMismatch_ReturnsBadRequest()
    {
        var service = new FakeTechnicService();
        var controller = new TechnicController(service);
        var item = NewTechnic();
        item.Id = 5;

        var result = await controller.Update(3, item, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_Success_ReturnsOk()
    {
        var service = new FakeTechnicService();
        var controller = new TechnicController(service);
        var item = NewTechnic();
        item.Id = 3;

        var result = await controller.Update(3, item, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_WhenServiceFails_ReturnsBadRequest()
    {
        var service = new FakeTechnicService
        {
            UpdateException = new InvalidOperationException("ошибка обновления")
        };
        var controller = new TechnicController(service);
        var item = NewTechnic();
        item.Id = 3;

        var result = await controller.Update(3, item, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Existing_ReturnsOk()
    {
        var service = new FakeTechnicService { DeleteResult = true };
        var controller = new TechnicController(service);

        var result = await controller.Delete(1, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Missing_ReturnsNotFound()
    {
        var service = new FakeTechnicService { DeleteResult = false };
        var controller = new TechnicController(service);

        var result = await controller.Delete(999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }
}
