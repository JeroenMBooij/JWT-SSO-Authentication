using Microsoft.AspNetCore.Identity;
using System;

namespace AuthenticationServer.Domain.Entities
{
    public class RoleEntity : IdentityRole<Guid>
    {
        public RoleEntity(string roleName)
            :base(roleName)
        {}

        public RoleEntity()
        {

        }
    }
}
