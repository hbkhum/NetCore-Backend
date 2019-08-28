using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace NetCore.Infrastructure.Context
{
    public class ApplicationRole : IdentityRole, IEnumerable
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}