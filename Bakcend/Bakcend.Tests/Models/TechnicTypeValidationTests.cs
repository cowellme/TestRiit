using Backend.Models;

namespace Backend.Tests.Models;

public class TechnicTypeValidationTests
{
    private static TechnicType Valid() => new() { Name = "Монитор" };

    [Fact]
    public void Name_128Characters_Accepted()
    {
        var item = Valid();

        item.Name = new string('a', 128);

        Assert.Equal(128, item.Name.Length);
    }

    [Fact]
    public void Name_LongerThan128_Throws()
    {
        var item = Valid();

        Assert.Throws<Exception>(() => item.Name = new string('a', 129));
    }

    [Fact]
    public void Name_Empty_Throws()
    {
        var item = Valid();

        Assert.Throws<Exception>(() => item.Name = " ");
    }
}
