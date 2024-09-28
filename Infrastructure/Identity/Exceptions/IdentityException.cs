namespace Infrastructure.Identity.Exceptions;

public class IdentityException : Exception
{
    public IdentityException(string message) : base(message)
    {
    }
}