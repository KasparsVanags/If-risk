using If_risk.Exceptions;

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
        var validRiskNames = AvailableRisks.Select(x => x.Name).ToList();
        var invalidRisks = selectedRisks.Where(x => !validRiskNames.Contains(x.Name)).ToList();
        if (invalidRisks.Any())
        {
            throw new CompanyDoesntInsureRiskException(invalidRisks);
        }

        try
        {
            GetPolicy(nameOfInsuredObject, validFrom);
            throw new PolicyAlreadyExistsException(nameOfInsuredObject, validFrom);
        }
        catch (PolicyNotFoundException)
        {

            _policies.Add(new Policy(nameOfInsuredObject, validFrom,
                validFrom.AddMonths(validMonths) - TimeSpan.FromDays(1), selectedRisks));
        }
        
        return _policies.Last();
    }

    public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom)
    {
        if (AvailableRisks.Any(x => x.Name == risk.Name.ToLower()))
        {
            var policyToEdit = GetPolicy(nameOfInsuredObject, validFrom);
            policyToEdit.InsuredRisks.Add(risk);
            policyToEdit.Premium += CalculatePremium(risk.YearlyPrice, validFrom, policyToEdit.ValidTill);

        }
        else
        {
            throw new CompanyDoesntInsureRiskException(risk);
        }
    }

    public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
    {
        var policy = _policies.FirstOrDefault(x => x.NameOfInsuredObject == nameOfInsuredObject && x.ValidFrom <= effectiveDate &&
                                                   effectiveDate < x.ValidTill, null);
        if(policy == null)
        {
            throw new PolicyNotFoundException(nameOfInsuredObject, effectiveDate);
        }
        
        return policy;
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