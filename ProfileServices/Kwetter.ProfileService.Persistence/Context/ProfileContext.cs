namespace Kwetter.ProfileService.Persistence.Context
{
    using Kwetter.ProfileService.Persistence.Entity;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Profile context class.
    /// </summary>
    public class ProfileContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
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
        /// Gets or sets the profile entities.
        /// </summary>
        public DbSet<ProfileEntity> Profiles { get; set; }
    }
}
