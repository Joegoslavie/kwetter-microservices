using Kwetter.AuthenticationService.Persistence.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.AuthenticationService.Persistence.Context
{
    public class AuthenticationContext : IdentityDbContext<KwetterUserEntity<int>, KwetterRoleEntity<int>, int>
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options)
            : base(options)
        {
        }
    }
}
