﻿namespace Kwetter.TweetService.Persistence.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the profile reference table.
        /// </summary>
        public virtual ProfileReferenceEntity Author { get; set; }

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
        /// Gets or sets the mentions to other users in the tweet.
        /// </summary>
        public List<MentionEntity> Mentions { get; set; } = new List<MentionEntity>();

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
