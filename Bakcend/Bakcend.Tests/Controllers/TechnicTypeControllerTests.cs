using Backend.Controllers;
using Backend.Models;
using Backend.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests.Controllers;

public class TechnicTypeControllerTests
{
    private static TechnicType NewType() => new() { Name = "Монитор" };

    [Fact]
    public async Task GetAll_ReturnsAllTypes()
    {
        var service = new FakeTechnicTypeService();
        service.All = new List<TechnicType> { NewType() };
        var controller = new TechnicTypeController(service);

        var result = await controller.GetAll(CancellationToken.None);

        Assert.Single(result);
    }

    [Fact]
    public async Task Create_Success_ReturnsOk()
    {
        var service = new FakeTechnicTypeService();
        var controller = new TechnicTypeController(service);

        var result = await controller.Create(NewType(), CancellationToken.None);

        Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(service.Added);
    }

    [Fact]
    public async Task Create_WhenServiceFails_ReturnsBadRequest()
    {
        var service = new FakeTechnicTypeService
        {
            AddException = new InvalidOperationException("ошибка добавления")
        };
        var controller = new TechnicTypeController(service);

        var result = await controller.Create(NewType(), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_IdMismatch_ReturnsBadRequest()
    {
        var service = new FakeTechnicTypeService();
        var controller = new TechnicTypeController(service);
        var item = NewType();
        item.Id = 5;

        var result = await controller.Update(3, item, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_Success_ReturnsOk()
    {
        var service = new FakeTechnicTypeService();
        var controller = new TechnicTypeController(service);
        var item = NewType();
        item.Id = 3;

        var result = await controller.Update(3, item, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Existing_ReturnsOk()
    {
        var service = new FakeTechnicTypeService { DeleteResult = true };
        var controller = new TechnicTypeController(service);

        var result = await controller.Delete(1, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Missing_ReturnsNotFound()
    {
        var service = new FakeTechnicTypeService { DeleteResult = false };
        var controller = new TechnicTypeController(service);

        var result = await controller.Delete(999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }
}
