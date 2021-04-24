namespace Kwetter.ProfileService.Persistence.Context
{
    using Kwetter.ProfileService.Persistence.Entity;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// 
    /// </summary>
    public class ProfileContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileContext"/> class.
        /// </summary>
        /// <param name="options"></param>
        public ProfileContext(DbContextOptions options)
            : base (options)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileContext"/> class.
        /// </summary>
        public ProfileContext()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<ProfileEntity> Profiles { get; set; }
    }
}
