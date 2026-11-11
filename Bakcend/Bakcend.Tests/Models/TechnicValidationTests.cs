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

    [Fact]
    public void AccountNumber_LatinLettersAndDigits_Accepted()
    {
        var item = Valid();

        item.AccountNumber = "Abc123";

        Assert.Equal("Abc123", item.AccountNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void AccountNumber_Empty_Throws(string value)
    {
        var item = Valid();

        Assert.Throws<Exception>(() => item.AccountNumber = value);
    }

    [Fact]
    public void AccountNumber_LongerThan32_Throws()
    {
        var item = Valid();

        Assert.Throws<Exception>(() => item.AccountNumber = new string('A', 33));
    }

    [Theory]
    [InlineData("АБВ")]
    [InlineData("abc-123")]
    [InlineData("abc 123")]
    [InlineData("abc_123")]
    public void AccountNumber_NotOnlyLatinLettersAndDigits_Throws(string value)
    {
        var item = Valid();

        Assert.Throws<Exception>(() => item.AccountNumber = value);
    }

    [Fact]
    public void Name_256Characters_Accepted()
    {
        var item = Valid();

        item.Name = new string('a', 256);

        Assert.Equal(256, item.Name.Length);
    }

    [Fact]
    public void Name_LongerThan256_Throws()
    {
        var item = Valid();

        Assert.Throws<Exception>(() => item.Name = new string('a', 257));
    }

    [Fact]
    public void Name_Empty_Throws()
    {
        var item = Valid();

        Assert.Throws<Exception>(() => item.Name = "  ");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1000)]
    public void RoomNumber_BoundaryValues_Accepted(int room)
    {
        var item = Valid();

        item.RoomNumber = room;

        Assert.Equal(room, item.RoomNumber);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1001)]
    [InlineData(-5)]
    public void RoomNumber_OutOfRange_Throws(int room)
    {
        var item = Valid();

        Assert.Throws<Exception>(() => item.RoomNumber = room);
    }
}
