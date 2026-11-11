using Backend.Dtos;
using Backend.Models;

namespace Backend.Tests.Dtos;

public class MappersTests
{
    [Fact]
    public void Technic_ToDto_MapsAllFields()
    {
        var item = new Technic
        {
            Id = 7,
            AccountNumber = "T001",
            Name = "ПК",
            TechnicTypeId = 2,
            RoomNumber = 305
        };

        var dto = item.ToDto();

        Assert.Equal(7, dto.Id);
        Assert.Equal("T001", dto.AccountNumber);
        Assert.Equal("ПК", dto.Name);
        Assert.Equal(2, dto.TechnicTypeId);
        Assert.Equal(305, dto.RoomNumber);
    }

    [Fact]
    public void CreateTechnicRequest_ToEntity_MapsAllFields()
    {
        var request = new CreateTechnicRequest
        {
            AccountNumber = "T001",
            Name = "ПК",
            TechnicTypeId = 2,
            RoomNumber = 305
        };

        var entity = request.ToEntity();

        Assert.Equal("T001", entity.AccountNumber);
        Assert.Equal("ПК", entity.Name);
        Assert.Equal(2, entity.TechnicTypeId);
        Assert.Equal(305, entity.RoomNumber);
    }

    [Fact]
    public void TechnicType_ToDto_MapsIdAndName()
    {
        var item = new TechnicType { Id = 3, Name = "Монитор" };

        var dto = item.ToDto();

        Assert.Equal(3, dto.Id);
        Assert.Equal("Монитор", dto.Name);
    }

    [Fact]
    public void CreateTechnicTypeRequest_ToEntity_MapsName()
    {
        var request = new CreateTechnicTypeRequest { Name = "Монитор" };

        var entity = request.ToEntity();

        Assert.Equal("Монитор", entity.Name);
    }
}