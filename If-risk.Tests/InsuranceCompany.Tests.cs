using If_risk.Exceptions;

namespace Tests;

[TestFixture]
public class InsuranceCompanyTests
{
    private InsuranceCompany _company;
    private static DateTime _testDate1;
    private static DateTime _testDate2;
    private Policy _policy;

    [SetUp]
    public void Setup()
    {
        SystemTime.SetDateTime(new DateTime(2000, 1, 1));
        _testDate1 = new DateTime(2022, 1, 24);
        _testDate2 = new DateTime(2023, 1, 23);
        _policy = new Policy("Ferrari", _testDate1, _testDate2,
            new List<Risk> { new("theft", 1000) });
        _company = new InsuranceCompany("TestCompany",
            new List<Risk> { new("theft", 1234), new("crash", 2000) },
            new List<Policy> { _policy });
    }

    [TearDown]
    public void TearDown()
    {
        SystemTime.ResetDateTime();
    }

    [Test]
    public void Can_Update_Available_Risks()
    {
        var newRisk = new Risk("fire", 1500);
        _company.AvailableRisks.Add(newRisk);
        _company.AvailableRisks.Should().Contain(newRisk);
    }

    [Test]
    public void Policy_Can_Be_Sold()
    {
        var policy = new Policy("House", _testDate1,
            _testDate2, new List<Risk> { new("theft", 1000) });
        _company.SellPolicy("House", _testDate1, 12,
            new List<Risk> { new("theft", 1000) }).Should().BeEquivalentTo(policy);
    }

    [Test]
    public void Multiple_Policies_For_Same_Object_Can_Be_Sold()
    {
        var policy = new Policy("House", _testDate1,
            _testDate2, new List<Risk> { new("theft", 1000) });
        _company.SellPolicy("House", _testDate1, 12,
            new List<Risk> { new("theft", 1000) }).Should().BeEquivalentTo(policy);
        var policy2 = new Policy("House", new DateTime(2025,1,1),
            new DateTime(2025,12,31), new List<Risk> { new("theft", 1000) });
        _company.SellPolicy("House", new DateTime(2025,1,1), 12,
            new List<Risk> { new("theft", 1000) }).Should().BeEquivalentTo(policy2);
        var policy3 = new Policy("House", new DateTime(2026,1,1),
            new DateTime(2026,12,31), new List<Risk> { new("theft", 1000) });
        _company.SellPolicy("House", new DateTime(2026,1,1), 12,
            new List<Risk> { new("theft", 1000) }).Should().BeEquivalentTo(policy3);
    }

    [Test]
    public void Cant_Sell_Policy_When_A_Policy_Within_The_Same_Time_Period_Already_Exists()
    {
        Assert.Throws<DuplicatePolicyException>(() => _company.SellPolicy("Ferrari",
            new DateTime(2022,12,31), 12, new List<Risk> { new("theft", 1000) }));
    }
    
    [Test]
    public void Cant_Sell_Policy_If_Company_Doesnt_Insure_Risk()
    {
        Assert.Throws<CompanyDoesntInsureRiskException>(() => _company.SellPolicy("House", _testDate1, 12,
            new List<Risk> { new("zombie attack", 1000) }));
    }

    [Test]
    public void Can_Get_Policy_Info()
    {
        _company.GetPolicy("Ferrari", _testDate1).Should().BeEquivalentTo(_policy);
    }
    
    [Test]
    public void Can_Add_Risks_To_Existing_Policy()
    {
        var newRisk = new Risk("crash", 1000);
        _company.AddRisk("Ferrari", newRisk, _testDate1 + TimeSpan.FromDays(31));
        _company.GetPolicy("Ferrari", _testDate1).InsuredRisks.Should().Contain(newRisk);
    }

    [Test]
    public void Cant_Add_Risk_If_A_Policy_Doesnt_Exist()
    {
        Assert.Throws<PolicyNotFoundException>(() => _company.AddRisk("Ghost",
            new Risk("crash", 1000), _testDate1 + TimeSpan.FromDays(31)));
    }
    
    [Test]
    public void Cant_Add_Risk_If_Company_Doesnt_Insure_That_Risk()
    {
        Assert.Throws<CompanyDoesntInsureRiskException>(() => _company.AddRisk("Ferrari",
            new Risk("alien attack", 1000), _testDate1 + TimeSpan.FromDays(31)));
    }

    [TestCaseSource(nameof(_invalidDates))]
    public void Cant_Add_Risk_If_Date_Is_Incorrect(DateTime date)
    {
        Assert.Throws<PolicyNotFoundException>(() => _company.AddRisk("Ferrari",
            new Risk("crash", 1000), new DateTime(2090,1,1)));
    }

    private static object[] _invalidDates =
    {
        new DateTime(2090,1,1),
        new DateTime(1980,1,1)
    };

    [Test]
    public void Adding_Risks_To_Policy_Adds_Premium()
    {
        var newRisk = new Risk("crash", 1000);
        _company.AddRisk("Ferrari", newRisk, _testDate1);
        _company.GetPolicy("Ferrari", _testDate1).Premium.Should().Be(2000);
    }
}