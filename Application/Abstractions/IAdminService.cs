using Application.DTO.Identity;

namespace Application.Abstractions;

public interface IAdminService
{
    Task CreateUserAsync(CreateUserRequest model);
    Task PromoteUserAsync(string userEmail);
    Task DemoteUserAsync(string userEmail);
    Task DeleteUserAsync(string userEmail); 
}