using Microsoft.AspNetCore.Identity;

namespace NetCore.Infrastructure.Context
{
    public class ApplicationUser : IdentityUser
    {

        //[ForeignKey("EmployeeId")]
        //public virtual Employee Employee { get; set; }
        //[Required]
        //public Guid EmployeeId { get; set; }
        //public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}