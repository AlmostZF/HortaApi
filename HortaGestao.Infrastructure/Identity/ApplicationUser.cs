using HortaGestao.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HortaGestao.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{ 
    public string Name { get; set; }
    public Guid? SystemUserId { get; set; }
    public SystemUserEntity? SystemUser { get; set; }

}