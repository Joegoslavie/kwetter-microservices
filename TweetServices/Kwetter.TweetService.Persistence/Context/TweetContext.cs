namespace Kwetter.TweetService.Persistence.Context
{
    using Kwetter.TweetService.Persistence.Entity;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Tweet context class.
    /// </summary>
    public class TweetContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TweetContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public TweetContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the tweet entities.
        /// </summary>
        public DbSet<TweetEntity> Tweets { get; set; }

        /// <summary>
        /// Gets or sets the hashtag entities.
        /// </summary>
        public DbSet<HashtagEntity> Hashtags { get; set; }

        /// <summary>
        /// Gets or sets the mention entities.
        /// </summary>
        public DbSet<MentionEntity> Mentions { get; set; }

        /// <summary>
        /// Gets or sets the like entities.
        /// </summary>
        public DbSet<LikeEntity> Likes { get; set; }

        /// <summary>
        /// Overrides creating models.
        /// </summary>
        /// <param name="modelBuilder">Used to build the models.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LikeEntity>().HasKey(t => new { t.TweetId, t.UserId });
        }
    }
}
