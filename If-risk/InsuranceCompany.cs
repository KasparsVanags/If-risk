namespace If_risk;

public class InsuranceCompany:IInsuranceCompany
{
    public string Name { get; }
    public IList<Risk> AvailableRisks { get; set; }
    private readonly IList<Policy> _policies;

    public InsuranceCompany(string name, IList<Risk> availableRisks)
    {
        Name = name;
        AvailableRisks = availableRisks;
        _policies = new List<Policy>();
    }
    
    public InsuranceCompany(string name, IList<Risk> availableRisks, IList<Policy> policyList)
    {
        Name = name;
        AvailableRisks = availableRisks;
        _policies = policyList;
    }

    public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
    {
        var invalidRisks = selectedRisks.Select(x => x.Name)
            .Except(AvailableRisks.Select(x => x.Name)).ToList();
        var alreadyExistingPolicy = _policies.Where(x => x.NameOfInsuredObject == nameOfInsuredObject)
            .Where(x => x.ValidTill >= validFrom).ToList();
        if (invalidRisks.Any())
        {
            throw new ArgumentException($"{string.Join(", ", invalidRisks)} not available");
        }

        if (alreadyExistingPolicy.Any())
        {
            throw new ArgumentException($"A policy already exists {alreadyExistingPolicy[0].ValidFrom} - " +
                                        $"{alreadyExistingPolicy[0].ValidTill}");
        }
        
        _policies.Add(new Policy(nameOfInsuredObject, validFrom,
            validFrom.AddMonths(validMonths) - TimeSpan.FromDays(1), selectedRisks));
        return _policies.Last();
    }

    public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom)
    {
        if (!_policies.Any(x => x.NameOfInsuredObject == nameOfInsuredObject &&
                               x.ValidFrom <= validFrom && validFrom < x.ValidTill))
        {
            throw new ArgumentException($"{nameOfInsuredObject} doesnt have a policy in {validFrom}");
        }
        
        if (AvailableRisks.Any(x => x.Name == risk.Name.ToLower()))
        {
            var policyToEdit = GetPolicy(nameOfInsuredObject, validFrom);
            policyToEdit.InsuredRisks.Add(risk);
            policyToEdit.Premium += CalculatePremium(risk.YearlyPrice, validFrom, policyToEdit.ValidTill);

        }
        else
        {
            throw new ArgumentException($"{risk} is not available");
        }
    }

    public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
    {
        return _policies.First(policy =>
            policy.NameOfInsuredObject == nameOfInsuredObject && policy.ValidTill > effectiveDate);
    }

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