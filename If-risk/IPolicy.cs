namespace If_risk;

public interface IPolicy 
{
    /// <summary>
    /// Name of insured object
    /// </summary>
    string NameOfInsuredObject { get; }
    /// <summary>
    /// Date when policy becomes active
    /// </summary>
    DateTime ValidFrom { get; }
    /// <summary>
    /// Date when policy becomes inactive
    /// </summary>
    DateTime ValidTill { get; }
    /// <summary>
    /// Total price of the policy. Calculate by summing up all insured risks.
    /// Take into account that risk price is given for 1 full year. Policy/risk period can be shorter.
    /// </summary>
    decimal Premium { get; }
    /// <summary>
    /// Initially included risks or risks at specific moment of time.
    /// </summary>
    IList<Risk> InsuredRisks { get; }
}