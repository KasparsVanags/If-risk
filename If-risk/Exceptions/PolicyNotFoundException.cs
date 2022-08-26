namespace If_risk.Exceptions;

[Serializable]
public class PolicyNotFoundException : Exception
{
    public PolicyNotFoundException()
    {
    }

    public PolicyNotFoundException(string? message) : base(message)
    {
    }

    public PolicyNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public PolicyNotFoundException(string nameOfInsuredObject, DateTime inDate) :
        base($"{nameOfInsuredObject} at {inDate}")
    {
    }
}