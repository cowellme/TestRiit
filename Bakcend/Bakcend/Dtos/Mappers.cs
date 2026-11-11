using Backend.Models;

namespace Backend.Dtos;

public static class Mappers
{
    public static TechnicDto ToDto(this Technic item) => new()
    {
        Id = item.Id,
        AccountNumber = item.AccountNumber,
        Name = item.Name,
        TechnicTypeId = item.TechnicTypeId,
        RoomNumber = item.RoomNumber
    };

    public static Technic ToEntity(this CreateTechnicRequest request) => new()
    {
        AccountNumber = request.AccountNumber,
        Name = request.Name,
        TechnicTypeId = request.TechnicTypeId,
        RoomNumber = request.RoomNumber
    };

    public static TechnicTypeDto ToDto(this TechnicType item) => new()
    {
        Id = item.Id,
        Name = item.Name
    };

    public static TechnicType ToEntity(this CreateTechnicTypeRequest request) => new()
    {
        Name = request.Name
    };
}