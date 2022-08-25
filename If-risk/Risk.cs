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

    public Risk(string name, decimal yearlyPrice)
    {
        if (name.Where(char.IsLetter).Count() < 3)
        {
            throw new FormatException("Invalid name");
        }

        if (yearlyPrice <= 0)
        {
            throw new ArgumentException("Invalid price");
        }
        
        Name = name.ToLower();
        YearlyPrice = yearlyPrice;
    }

    public override string ToString()
    {
        return Name;
    }
}