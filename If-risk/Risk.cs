using If_risk.Exceptions;

namespace If_risk;

public struct Risk 
{
    /// <summary>
    /// Unique name of the risk
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Risk yearly price
    /// </summary>
    public decimal YearlyPrice { get; set; }

    public DateTime CreationDate { get; }

    public Risk(string name, decimal yearlyPrice)
    {
        if (name.Where(char.IsLetter).Count() < 3)
        {
            throw new InvalidRiskException("Invalid name");
        }

        if (yearlyPrice <= 0)
        {
            throw new InvalidRiskException("Invalid price");
        }

        CreationDate = new DateTime();
        Name = name.ToLower();
        YearlyPrice = yearlyPrice;
    }
    
    public Risk(string name, decimal yearlyPrice, DateTime creationDate)
    {
        if (name.Where(char.IsLetter).Count() < 3)
        {
            throw new InvalidRiskException("Invalid name");
        }

        if (yearlyPrice <= 0)
        {
            throw new InvalidRiskException("Invalid price");
        }

        CreationDate = creationDate;
        Name = name.ToLower();
        YearlyPrice = yearlyPrice;
    }

    public override string ToString()
    {
        return Name;
    }
}