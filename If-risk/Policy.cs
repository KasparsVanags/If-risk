using System.Text.RegularExpressions;
using If_risk.Exceptions;

namespace If_risk;

public class Policy:IPolicy
{
    public string NameOfInsuredObject { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTill { get; }
    public decimal Premium
    {
        get
        {
            return InsuredRisks.Select(x => PremiumCalculator.CalculatePremium
                (x.YearlyPrice, x.CreationDate == default ? ValidFrom : x.CreationDate, ValidTill)).Sum();
        }
    }

    public IList<Risk> InsuredRisks { get; }
    
    public Policy(string nameOfInsuredObject, DateTime validFrom, DateTime validTill, 
        IList<Risk> insuredRisks)
    {
        if (IsValid(nameOfInsuredObject, validFrom, validTill, insuredRisks))
        {
            NameOfInsuredObject = nameOfInsuredObject;
            ValidFrom = validFrom;
            ValidTill = validTill;
            InsuredRisks = insuredRisks;
        }
        else
        {
            throw new InvalidPolicyException();
        }
    }

    private bool IsValid(string nameOfInsuredObject, DateTime validFrom, DateTime validTill, 
        IList<Risk> insuredRisks)
    {
        return Regex.IsMatch(nameOfInsuredObject, @"^[a-zA-Z0-9]|\s+$") &&
               nameOfInsuredObject.Where(char.IsLetter).Count() >= 3 &&
               validFrom < validTill &&
               insuredRisks.Count >= 1 &&
               validFrom >= SystemTime.Now();
    }
}