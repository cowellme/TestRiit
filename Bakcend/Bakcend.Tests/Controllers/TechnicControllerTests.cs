using Backend.Controllers;
using Backend.Dtos;
using Backend.Models;
using Backend.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Backend.Tests.Controllers;

public class TechnicControllerTests
{
    private static TechnicController CreateController(FakeTechnicService service) =>
        new(service, NullLogger<TechnicController>.Instance);

    private static CreateTechnicRequest NewRequest() => new()
    {
        AccountNumber = "T001",
        Name = "Тест",
        TechnicTypeId = 1,
        RoomNumber = 101
    };

    private static Technic NewTechnic() => new()
    {
        Id = 7,
        AccountNumber = "T001",
        Name = "Тест",
        TechnicTypeId = 1,
        RoomNumber = 101
    };

    [Fact]
    public async Task GetAll_Empty_ReturnsOkWithEmptyList()
    {
        var service = new FakeTechnicService();
        var controller = CreateController(service);

        var result = await controller.GetAll(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Empty((IEnumerable<TechnicDto>)ok.Value!);
    }

    [Fact]
    public async Task GetAll_WithItems_ReturnsDtos()
    {
        var service = new FakeTechnicService();
        service.All = new List<Technic> { NewTechnic() };
        var controller = CreateController(service);

        var result = await controller.GetAll(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var items = Assert.IsAssignableFrom<IReadOnlyList<TechnicDto>>(ok.Value);
        Assert.Single(items);
    }

    [Fact]
    public async Task Get_Existing_ReturnsDto()
    {
        var service = new FakeTechnicService { ById = NewTechnic() };
        var controller = CreateController(service);

        var result = await controller.Get(7, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<TechnicDto>(ok.Value);
    }

    [Fact]
    public async Task Get_Missing_ReturnsNotFound()
    {
        var service = new FakeTechnicService { ById = null };
        var controller = CreateController(service);

        var result = await controller.Get(999, CancellationToken.None);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_Success_ReturnsDto()
    {
        var service = new FakeTechnicService();
        var controller = CreateController(service);

        var result = await controller.Create(NewRequest(), CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<TechnicDto>(ok.Value);
        Assert.NotNull(service.Added);
    }

    [Fact]
    public async Task Create_WhenLimitReached_ReturnsBadRequest()
    {
        var service = new FakeTechnicService
        {
            AddException = new InvalidOperationException("Лимит не более 2000 записей")
        };
        var controller = CreateController(service);

        var result = await controller.Create(NewRequest(), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_WhenDuplicate_ReturnsConflict()
    {
        var service = new FakeTechnicService
        {
            AddException = new DbUpdateException("duplicate", (Exception?)null)
        };
        var controller = CreateController(service);

        var result = await controller.Create(NewRequest(), CancellationToken.None);

        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_Success_SetsIdAndReturnsDto()
    {
        var service = new FakeTechnicService();
        var controller = CreateController(service);

        var result = await controller.Update(7, new UpdateTechnicRequest
        {
            AccountNumber = "T001",
            Name = "Тест",
            TechnicTypeId = 1,
            RoomNumber = 101
        }, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<TechnicDto>(ok.Value);
        Assert.NotNull(service.Updated);
        Assert.Equal(7, service.Updated!.Id);
    }

    [Fact]
    public async Task Update_WhenServiceFails_ReturnsBadRequest()
    {
        var service = new FakeTechnicService
        {
            UpdateException = new InvalidOperationException("ошибка обновления")
        };
        var controller = CreateController(service);

        var result = await controller.Update(7, new UpdateTechnicRequest
        {
            AccountNumber = "T001",
            Name = "Тест",
            TechnicTypeId = 1,
            RoomNumber = 101
        }, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Existing_ReturnsOk()
    {
        var service = new FakeTechnicService { DeleteResult = true };
        var controller = CreateController(service);

        var result = await controller.Delete(1, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Missing_ReturnsNotFound()
    {
        var service = new FakeTechnicService { DeleteResult = false };
        var controller = CreateController(service);

        var result = await controller.Delete(999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }
}