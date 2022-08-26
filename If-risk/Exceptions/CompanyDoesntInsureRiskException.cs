namespace If_risk.Exceptions;

[Serializable]
public class CompanyDoesntInsureRiskException : Exception
{
    public CompanyDoesntInsureRiskException()
    {
    }

    public CompanyDoesntInsureRiskException(string? message) : base(message)
    {
    }

    public CompanyDoesntInsureRiskException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public CompanyDoesntInsureRiskException(Risk risk) :
        base(risk.ToString())
    {
    }
    
    public CompanyDoesntInsureRiskException(IEnumerable<Risk> risk) :
        base(string.Join(", ", risk.Select(x => x.ToString())))
    {
    }
}