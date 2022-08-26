using If_risk.Exceptions;

namespace Tests;

[TestFixture]
public class RiskTests
{
    [Test]
    public void ValidRiskCanBeCreated()
    {
        var risk = new Risk("Fire", 1000);
        risk.Name.Should().Be("fire");
        risk.YearlyPrice.Should().Be(1000);
    }
    
    [Test]
    [TestCase("a", 100)]
    [TestCase("a!#12", 100)]
    [TestCase("!!!!!!!!", 100)]
    [TestCase("1111111", 100)]
    [TestCase("fire", 0)]
    [TestCase("crash", -10)]
    [TestCase("a", -100)]
    public void Invalid_Risks_Cant_Be_Created(string name, decimal yearlyPrice)
    {
        Action a = () => new Risk(name, yearlyPrice);
        a.Should().Throw<InvalidRiskException>();
    }
}