namespace Kwetter.FollowingService.Persistence.Context
{
    using Kwetter.FollowingService.Persistence.Entity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// FollowingContext class.
    /// </summary>
    public class FollowingContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowingContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public FollowingContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowingContext"/> class.
        /// </summary>
        public FollowingContext()
        {
        }

        /// <summary>
        /// Gets or sets the blocked by users records.
        /// </summary>
        public DbSet<BlockEntity> Blocked { get; set; }

        /// <summary>
        /// Gets or sets the followings by users records.
        /// </summary>
        public DbSet<FollowingEntity> Followings { get; set; }
    }
}
