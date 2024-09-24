namespace Infrastructure.Identity;

public class IdentityException : Exception
{
    public IdentityException(string message) : base(message)
    {
    }
}