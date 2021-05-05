namespace Kwetter.TweetService.Persistence.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    /// <summary>
    /// Represents a mention record.
    /// </summary>
    public class MentionEntity
    {
        /// <summary>
        /// Gets or sets the key of the mention.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the tweet id.
        /// </summary>
        public int TweetId { get; set; }

        /// <summary>
        /// Gets or sets the profile reference table.
        /// </summary>
        public virtual ProfileReferenceEntity DirectedTo { get; set; }

        /// <summary>
        /// Gets or sets the accociated tweet.
        /// </summary>
        public virtual TweetEntity Tweet { get; set; }
    }
}
