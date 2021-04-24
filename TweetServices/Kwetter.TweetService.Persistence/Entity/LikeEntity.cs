namespace Kwetter.TweetService.Persistence.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a like record.
    /// </summary>
    public class LikeEntity
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
