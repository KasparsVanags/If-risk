using System.Text.RegularExpressions;
using If_risk.Exceptions;

namespace If_risk;

public class Policy:IPolicy
{
    public string NameOfInsuredObject { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTill { get; }
    public decimal Premium { get; set; }
    public IList<Risk> InsuredRisks { get; }
    
    public Policy(string nameOfInsuredObject, DateTime validFrom, DateTime validTill, 
        IList<Risk> insuredRisks)
    {
        if (!Regex.IsMatch(nameOfInsuredObject, @"^[a-zA-Z0-9]|\s+$") ||
            nameOfInsuredObject.Where(char.IsLetter).Count() < 3)
        {
            throw new InvalidPolicyException("Invalid name");
        }

        if (validFrom >= validTill)
        {
            throw new InvalidPolicyException("Start date can't be before or same as end date");
        }

        if (insuredRisks.Count < 1)
        {
            throw new InvalidPolicyException("No risks selected");
        }

        if (validFrom < SystemTime.Now())
        {
            throw new InvalidPolicyException("Date can't be in past");
        }
        
        NameOfInsuredObject = nameOfInsuredObject;
        ValidFrom = validFrom;
        ValidTill = validTill;
        Premium = insuredRisks.Select(risk => 
            InsuranceCompany.CalculatePremium(risk.YearlyPrice, validFrom, validTill)).Sum();
        InsuredRisks = insuredRisks;
    }
}