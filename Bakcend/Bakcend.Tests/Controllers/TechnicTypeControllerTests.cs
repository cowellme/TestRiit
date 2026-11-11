using Backend.Controllers;
using Backend.Dtos;
using Backend.Models;
using Backend.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace Backend.Tests.Controllers;

public class TechnicTypeControllerTests
{
    private static TechnicTypeController CreateController(FakeTechnicTypeService service) =>
        new(service, NullLogger<TechnicTypeController>.Instance);

    private static CreateTechnicTypeRequest NewRequest() => new() { Name = "Монитор" };

    private static TechnicType NewType() => new() { Id = 3, Name = "Монитор" };

    [Fact]
    public async Task GetAll_Empty_ReturnsOkWithEmptyList()
    {
        var service = new FakeTechnicTypeService();
        var controller = CreateController(service);

        var result = await controller.GetAll(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Empty((IEnumerable<TechnicTypeDto>)ok.Value!);
    }

    [Fact]
    public async Task GetAll_WithItems_ReturnsDtos()
    {
        var service = new FakeTechnicTypeService();
        service.All = new List<TechnicType> { NewType() };
        var controller = CreateController(service);

        var result = await controller.GetAll(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Single(Assert.IsAssignableFrom<IReadOnlyList<TechnicTypeDto>>(ok.Value));
    }

    [Fact]
    public async Task Create_Success_ReturnsDto()
    {
        var service = new FakeTechnicTypeService();
        var controller = CreateController(service);

        var result = await controller.Create(NewRequest(), CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<TechnicTypeDto>(ok.Value);
        Assert.NotNull(service.Added);
    }

    [Fact]
    public async Task Create_WhenServiceFails_ReturnsBadRequest()
    {
        var service = new FakeTechnicTypeService
        {
            AddException = new InvalidOperationException("ошибка добавления")
        };
        var controller = CreateController(service);

        var result = await controller.Create(NewRequest(), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_Success_SetsIdAndReturnsDto()
    {
        var service = new FakeTechnicTypeService();
        var controller = CreateController(service);

        var result = await controller.Update(3, NewRequest(), CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<TechnicTypeDto>(ok.Value);
        Assert.NotNull(service.Updated);
        Assert.Equal(3, service.Updated!.Id);
    }

    [Fact]
    public async Task Update_WhenServiceFails_ReturnsBadRequest()
    {
        var service = new FakeTechnicTypeService
        {
            UpdateException = new InvalidOperationException("ошибка обновления")
        };
        var controller = CreateController(service);

        var result = await controller.Update(3, NewRequest(), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Existing_ReturnsOk()
    {
        var service = new FakeTechnicTypeService { DeleteResult = true };
        var controller = CreateController(service);

        var result = await controller.Delete(1, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Missing_ReturnsNotFound()
    {
        var service = new FakeTechnicTypeService { DeleteResult = false };
        var controller = CreateController(service);

        var result = await controller.Delete(999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }
}