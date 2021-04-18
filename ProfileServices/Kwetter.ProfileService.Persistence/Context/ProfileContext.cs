namespace Kwetter.ProfileService.Persistence.Context
{
    using Kwetter.ProfileService.Persistence.Entity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProfileContext : DbContext
    {
        public ProfileContext(DbContextOptions options)
            : base (options)
        {

        }

        public ProfileContext()
        {
        }

        public DbSet<ProfileEntity> Profiles { get; set; }
    }
}
