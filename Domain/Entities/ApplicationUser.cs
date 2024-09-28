using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Audit> Audits { get; set; } = null!;
}