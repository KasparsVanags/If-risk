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
            throw new DuplicatePolicyException(nameOfInsuredObject, validFrom);
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
            policyToEdit.InsuredRisks.Add(new Risk(risk.Name, risk.YearlyPrice, validFrom));
        }
        else
        {
            throw new CompanyDoesntInsureRiskException(risk);
        }
    }

    public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
    {
        var policy = _policies.FirstOrDefault(x => x!.NameOfInsuredObject == nameOfInsuredObject && x.ValidFrom <= effectiveDate &&
                                                   effectiveDate < x.ValidTill, null);
        if(policy == null)
        {
            throw new PolicyNotFoundException(nameOfInsuredObject, effectiveDate);
        }
        
        return policy;
    }
}