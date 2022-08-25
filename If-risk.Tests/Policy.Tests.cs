namespace Tests;

[TestFixture]
public class PolicyTests
{
    private static DateTime _testDate1;
    private static DateTime _testDate2;
    
    [SetUp]
    public void Setup()
    {
        SystemTime.SetDateTime(new DateTime(2000,1,1));
        _testDate1 = new DateTime(2022, 1, 24);
        _testDate2 = new DateTime(2023, 1, 24);
    }

    [TearDown]
    public void TearDown()
    {
        SystemTime.ResetDateTime();
    }

    [Test]
    public void Valid_Policy_Can_Be_Created()
    {
        var testRisk = new Risk("Fire", 1000);
        var policy = new Policy("Johns car", _testDate1, _testDate2,
            new List<Risk>{ testRisk, new("Crash", 1000) });
        policy.NameOfInsuredObject.Should().Be("Johns car");
        policy.ValidFrom.Should().Be(_testDate1);
        policy.ValidTill.Should().Be(_testDate2);
        policy.InsuredRisks.Should().Contain(testRisk);
    }
    
    [Test]
    public void Premium_Is_Calculated_Correctly_When_A_Policy_Is_Created()
    {
        var policy = new Policy("house", new DateTime(2022, 1, 1),
            new DateTime(2022, 12, 31), new List<Risk>{new("fire", 1000)});
        policy.Premium.Should().Be(1000);
    }
    
    [TestCaseSource(nameof(_invalidPolicyTestCases))]
    public void InvalidPoliciesCantBeCreated(string nameOfInsuredObject, DateTime validFrom, DateTime validTill,
        IList<Risk> insuredRisks)
    {
        
        Action a = () => new Policy(nameOfInsuredObject, validFrom, validTill, insuredRisks);
        a.Should().Throw<Exception>();
    }

    private static object[] _invalidPolicyTestCases =
    {
        new object[] { "a", _testDate1, _testDate2,
            new List<Risk>{ new("Fire", 1000), new("Crash", 1000) }},
        new object[] { "!!!!!!!", _testDate1, _testDate2,
            new List<Risk>{ new("Fire", 1000), new("Crash", 1000) }},
        new object[] { "2222222", _testDate1, _testDate2,
            new List<Risk>{ new("Fire", 1000), new("Crash", 1000) }},
        new object[] { "#house", _testDate1, _testDate2,
            new List<Risk>{ new("Fire", 1000), new("Crash", 1000) }},
        new object[] { "house", _testDate1, _testDate2,
            new List<Risk>()},
        new object[] { "house", _testDate1, _testDate1,
            new List<Risk>{ new("Fire", 1000), new("Crash", 1000) }},
        new object[] { "house", _testDate2, _testDate1,
            new List<Risk>{ new("Fire", 1000), new("Crash", 1000) }},
        new object[] { "house", new DateTime(1420,6,6), _testDate1,
            new List<Risk>{ new("Fire", 1000), new("Crash", 1000) }}
    };
}