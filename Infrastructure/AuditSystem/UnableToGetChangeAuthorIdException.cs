namespace Infrastructure.AuditSystem;

public class UnableToGetChangeAuthorIdException(string message) : Exception(message)
{
}