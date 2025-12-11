
using DDDPractice.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DDD_Practice.DDDPractice.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{ 
    public string Name { get; set; }
//    public string LastName { get; set; }

    public Guid? SystemUserId { get; set; }
    public SystemUserEntity? SystemUser { get; set; }

}