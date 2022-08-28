using NUnit.Framework;

namespace Tests;

[TestFixture]
public class PremiumCalculator_Tests
{
    [TestCaseSource(nameof(_premiumCalculationTestCases))]
    public void Premium_Calculation_Is_Correct(decimal yearlyPremium, DateTime startDate, DateTime endDate, decimal result)
    {
        PremiumCalculator.CalculatePremium(yearlyPremium, startDate, endDate).Should().Be(result);
    }

    private static object[] _premiumCalculationTestCases =
    {
        //normal years only
        new object[]
        {
            (decimal)1200, new DateTime(2022, 1, 1), new DateTime(2022, 12, 31), (decimal)1200
        },
        new object[]
        {
            (decimal)1145.23, new DateTime(2022, 1, 1), new DateTime(2022, 12, 31), (decimal)1145.23
        },
        new object[]
        {
            (decimal)1200, new DateTime(2022, 1, 1), new DateTime(2023, 12, 31), (decimal)2400
        },
        new object[]
        {
            (decimal)1200, new DateTime(2021, 1, 1), new DateTime(2023, 12, 31), (decimal)3600
        },
        new object[]
        {
            (decimal)1200, new DateTime(2022, 1, 1), new DateTime(2022, 3, 14), (decimal)240
        },
        //leap years only
        new object[]
        {
            (decimal)1200, new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), (decimal)1200
        },
        new object[]
        {
            (decimal)1234.56, new DateTime(2020, 1, 1), new DateTime(2020, 12, 31), (decimal)1234.56
        },
        new object[]
        {
            (decimal)1200, new DateTime(2020, 1, 1), new DateTime(2020, 5, 1), (decimal)400
        },
        //both
        new object[]
        {
            (decimal)1200, new DateTime(2020, 1, 1), new DateTime(2022, 12, 31), (decimal)3600
        },
        new object[]
        {
            (decimal)1200, new DateTime(2022, 1, 1), new DateTime(2024, 12, 31), (decimal)3600
        },
        new object[]
        {
            (decimal)1233, new DateTime(2020, 1, 1), new DateTime(2029, 12, 31), (decimal)12330
        },
        new object[]
        {
            (decimal)1200, new DateTime(2020, 1, 1), new DateTime(2021, 12, 31), (decimal)2400
        },
        new object[]
        {
            (decimal)300, new DateTime(2020, 11, 1), new DateTime(2021, 3, 14), (decimal)110
        },
        new object[]
        {
            (decimal)300, new DateTime(2019, 10, 20), new DateTime(2020, 3, 1), (decimal)110
        },
        new object[]
        {
            (decimal)300, new DateTime(2016, 11, 1), new DateTime(2021, 3, 14), (decimal)1310
        },
        new object[]
        {
            (decimal)300, new DateTime(2019, 10, 20), new DateTime(2024, 3, 1), (decimal)1310
        }
    };

    [Test]
    public void Cant_Calculate_Premium_When_Both_Dates_Are_The_Same()
    {
        var date = new DateTime(2022, 1, 24);
        Assert.Throws<ArgumentException>(() => 
            PremiumCalculator.CalculatePremium(1000, date, date));
    }
    
    [Test]
    public void Cant_Calculate_Premium_When_End_Date_Is_Before_Start_Date()
    {
        Assert.Throws<ArgumentException>(() => 
            PremiumCalculator.CalculatePremium(1000, new DateTime(2023, 1, 23),
                new DateTime(2022, 1, 24)));
    }
}