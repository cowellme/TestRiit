using Backend.Models;
using Backend.Services;
using Backend.Tests.Fakes;

namespace Backend.Tests.Services;

public class TechnicTypeServiceTests
{
    private static TechnicType NewTechnic() => new()
    {
        Name = "Тест"
    };


    [Fact]
    public async Task AddRange_WhenAtLimitAndEmptyList_Adds()
    {
        var repository = new FakeRepository<TechnicType, int> { Count = 2000 };
        var service = new TechnicTypeService(repository);

        await service.AddRange(new List<TechnicType>(), CancellationToken.None);

        Assert.Equal(1, repository.AddRangeCalls);
    }

    [Fact]
    public async Task GetAll_DelegatesToRepository()
    {
        var repository = new FakeRepository<TechnicType, int>();
        repository.All.Add(NewTechnic());
        var service = new TechnicTypeService(repository);

        var result = await service.GetAll(CancellationToken.None);

        Assert.Single(result);
    }

    [Fact]
    public async Task GetById_DelegatesToRepository()
    {
        var repository = new FakeRepository<TechnicType, int> { GetByIdResult = NewTechnic() };
        var service = new TechnicTypeService(repository);

        var result = await service.GetById(1, CancellationToken.None);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task Update_DelegatesToRepository()
    {
        var repository = new FakeRepository<TechnicType, int>();
        var service = new TechnicTypeService(repository);

        var result = await service.Update(NewTechnic(), CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task Delete_DelegatesToRepository()
    {
        var repository = new FakeRepository<TechnicType, int>();
        var service = new TechnicTypeService(repository);

        var result = await service.Delete(1, CancellationToken.None);

        Assert.True(result);
    }
}
