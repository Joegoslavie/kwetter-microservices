namespace Kwetter.TweetService.Persistence.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    /// <summary>
    /// Represents a tweet record.
    /// </summary>
    public class TweetEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the avatar url.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the tweet contents.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the time the tweet was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the amount of likes this tweet has.
        /// </summary>
        [NotMapped]
        public int Likes { get; set; }
    }
}
