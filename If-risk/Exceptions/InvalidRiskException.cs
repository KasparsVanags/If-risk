namespace If_risk.Exceptions;

[Serializable]
public class InvalidRiskException : Exception
{
    public InvalidRiskException()
    {
    }

    public InvalidRiskException(string? message) : base(message)
    {
    }

    public InvalidRiskException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}