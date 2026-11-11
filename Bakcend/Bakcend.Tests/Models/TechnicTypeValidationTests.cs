using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Tests.Models;

public class TechnicTypeValidationTests
{
    private static TechnicType Valid() => new() { Name = "Монитор" };

    private static bool IsValid(object value) =>
        Validator.TryValidateObject(value, new ValidationContext(value), null, validateAllProperties: true);

    [Fact]
    public void Name_128Characters_IsValid()
    {
        var item = Valid();
        item.Name = new string('a', 128);

        Assert.True(IsValid(item));
    }

    [Fact]
    public void Name_129Characters_IsNotValid()
    {
        var item = Valid();
        item.Name = new string('a', 129);

        Assert.False(IsValid(item));
    }

    [Fact]
    public void Name_Empty_IsNotValid()
    {
        var item = Valid();
        item.Name = "";

        Assert.False(IsValid(item));
    }
}