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
        /// Gets or sets the author user id.
        /// </summary>
        public int UserId { get; set; }

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
        /// Gets or sets the likes of the tweet.
        /// </summary>
        public List<LikeEntity> LikedBy { get; set; } = new List<LikeEntity>();

        /// <summary>
        /// Gets or sets the hashtags in the tweet content.
        /// </summary>
        public List<HashtagEntity> Hashtags { get; set; } = new List<HashtagEntity>();

        /// <summary>
        /// Gets or sets a value indicating whether if the tweet is flagged.
        /// </summary>
        public bool Reported { get; set; }

        /// <summary>
        /// Gets or sets the time the tweet was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
