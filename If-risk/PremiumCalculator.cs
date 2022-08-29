namespace If_risk;

public static class PremiumCalculator
{
    public static decimal CalculatePremium(decimal yearlyPremium, DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException("Start date cant be higher than end date");
        }
        
        decimal premium = 0;
        var daysInYear = DateTime.IsLeapYear(startDate.Year) ? 366 : 365;
        if (startDate.Year == endDate.Year)
        {
            return (yearlyPremium / daysInYear) * (decimal)((endDate - startDate).Days + 1);
        }
        
        premium += (yearlyPremium / daysInYear) *
                   (decimal)((new DateTime(startDate.Year, 12, 31) - startDate).Days + 1);
        daysInYear = DateTime.IsLeapYear(endDate.Year) ? 366 : 365;
        premium += (yearlyPremium / daysInYear) *
                   (decimal)((endDate - new DateTime(endDate.Year, 1, 1)).Days + 1);
        for (var i = startDate.Year + 1; i < endDate.Year; i++)
        {
            premium += yearlyPremium;
        }

        return premium;
    }
}