namespace Kwetter.TweetService.Persistence.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents a mention record.
    /// </summary>
    public class MentionEntity
    {
        /// <summary>
        /// Gets or sets the tweet id.
        /// </summary>
        public int TweetId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }
    }
}
