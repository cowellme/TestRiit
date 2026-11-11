using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Tests.Models;

public class TechnicValidationTests
{
    private static Technic Valid() => new()
    {
        AccountNumber = "T001",
        Name = "ПК офисный",
        TechnicTypeId = 1,
        RoomNumber = 101
    };

    private static bool IsValid(object value) =>
        Validator.TryValidateObject(value, new ValidationContext(value), null, validateAllProperties: true);

    [Fact]
    public void Valid_Technic_PassesValidation()
    {
        Assert.True(IsValid(Valid()));
    }

    [Theory]
    [InlineData("")]
    [InlineData("АБВ")]
    [InlineData("abc-123")]
    [InlineData("abc 123")]
    public void AccountNumber_Invalid_IsNotValid(string value)
    {
        var item = Valid();
        item.AccountNumber = value;

        Assert.False(IsValid(item));
    }

    [Fact]
    public void AccountNumber_32Characters_IsValid()
    {
        var item = Valid();
        item.AccountNumber = new string('A', 32);

        Assert.True(IsValid(item));
    }

    [Fact]
    public void AccountNumber_33Characters_IsNotValid()
    {
        var item = Valid();
        item.AccountNumber = new string('A', 33);

        Assert.False(IsValid(item));
    }

    [Fact]
    public void Name_256Characters_IsValid()
    {
        var item = Valid();
        item.Name = new string('a', 256);

        Assert.True(IsValid(item));
    }

    [Fact]
    public void Name_257Characters_IsNotValid()
    {
        var item = Valid();
        item.Name = new string('a', 257);

        Assert.False(IsValid(item));
    }

    [Fact]
    public void Name_Empty_IsNotValid()
    {
        var item = Valid();
        item.Name = "";

        Assert.False(IsValid(item));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1001)]
    public void RoomNumber_OutOfRange_IsNotValid(int room)
    {
        var item = Valid();
        item.RoomNumber = room;

        Assert.False(IsValid(item));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1000)]
    public void RoomNumber_InRange_IsValid(int room)
    {
        var item = Valid();
        item.RoomNumber = room;

        Assert.True(IsValid(item));
    }
}