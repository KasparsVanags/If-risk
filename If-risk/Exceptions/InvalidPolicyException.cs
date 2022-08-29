namespace If_risk.Exceptions;

public class InvalidPolicyException : Exception
{
    public InvalidPolicyException()
    {
    }

    public InvalidPolicyException(string? message) : base(message)
    {
    }

    public InvalidPolicyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}