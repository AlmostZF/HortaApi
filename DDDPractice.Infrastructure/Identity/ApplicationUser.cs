
using DDDPractice.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DDDPractice.DDDPractice.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{ 
    public string Name { get; set; }
    public Guid? SystemUserId { get; set; }
    public SystemUserEntity? SystemUser { get; set; }

}