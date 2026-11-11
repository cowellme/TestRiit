using Backend.Models;
using Backend.Services;
using Backend.Tests.Fakes;

namespace Backend.Tests.Services;

public class TechnicServiceTests
{
    private static Technic NewTechnic() => new()
    {
        AccountNumber = "T001",
        Name = "Тест",
        TechnicTypeId = 1,
        RoomNumber = 101
    };

    [Fact]
    public async Task Add_WhenCountIs1999_AddsEntity()
    {
        var repository = new FakeRepository<Technic, int> { Count = 1999 };
        var service = new TechnicService(repository);

        var result = await service.Add(NewTechnic(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, repository.AddCalls);
    }

    [Fact]
    public async Task Add_WhenCountIs2000_ThrowsAndDoesNotAdd()
    {
        var repository = new FakeRepository<Technic, int> { Count = 2000 };
        var service = new TechnicService(repository);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.Add(NewTechnic(), CancellationToken.None));

        Assert.Equal(0, repository.AddCalls);
    }

    [Fact]
    public async Task AddRange_WhenTotalIsExactly2000_Adds()
    {
        var repository = new FakeRepository<Technic, int> { Count = 1995 };
        var service = new TechnicService(repository);
        var items = Enumerable.Repeat(NewTechnic(), 5).ToList();

        await service.AddRange(items, CancellationToken.None);

        Assert.Equal(1, repository.AddRangeCalls);
    }

    [Fact]
    public async Task AddRange_WhenTotalExceeds2000_Throws()
    {
        var repository = new FakeRepository<Technic, int> { Count = 1995 };
        var service = new TechnicService(repository);
        var items = Enumerable.Repeat(NewTechnic(), 6).ToList();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.AddRange(items, CancellationToken.None));

        Assert.Equal(0, repository.AddRangeCalls);
    }

    [Fact]
    public async Task AddRange_WhenAtLimitAndEmptyList_Adds()
    {
        var repository = new FakeRepository<Technic, int> { Count = 2000 };
        var service = new TechnicService(repository);

        await service.AddRange(new List<Technic>(), CancellationToken.None);

        Assert.Equal(1, repository.AddRangeCalls);
    }

    [Fact]
    public async Task GetAll_DelegatesToRepository()
    {
        var repository = new FakeRepository<Technic, int>();
        repository.All.Add(NewTechnic());
        var service = new TechnicService(repository);

        var result = await service.GetAll(CancellationToken.None);

        Assert.Single(result);
    }

    [Fact]
    public async Task GetById_DelegatesToRepository()
    {
        var repository = new FakeRepository<Technic, int> { GetByIdResult = NewTechnic() };
        var service = new TechnicService(repository);

        var result = await service.GetById(1, CancellationToken.None);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task Update_DelegatesToRepository()
    {
        var repository = new FakeRepository<Technic, int>();
        var service = new TechnicService(repository);

        var result = await service.Update(NewTechnic(), CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task Delete_DelegatesToRepository()
    {
        var repository = new FakeRepository<Technic, int>();
        var service = new TechnicService(repository);

        var result = await service.Delete(1, CancellationToken.None);

        Assert.True(result);
    }
}
