namespace If_risk.Exceptions;

public class DuplicatePolicyException : Exception
{
    public DuplicatePolicyException()
    {
    }

    public DuplicatePolicyException(string? message) : base(message)
    {
    }

    public DuplicatePolicyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public DuplicatePolicyException(string nameOfInsuredObject, DateTime inDate) :
        base($"{nameOfInsuredObject} at {inDate}")
    {
    }
}