namespace If_risk.Exceptions;

[Serializable]
public class PolicyAlreadyExistsException : Exception
{
    public PolicyAlreadyExistsException()
    {
    }

    public PolicyAlreadyExistsException(string? message) : base(message)
    {
    }

    public PolicyAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public PolicyAlreadyExistsException(string nameOfInsuredObject, DateTime inDate) :
        base($"{nameOfInsuredObject} at {inDate}")
    {
    }
}