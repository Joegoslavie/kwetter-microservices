namespace Kwetter.TweetService.Persistence.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a like record.
    /// </summary>
    public class LikeEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the tweet id.
        /// </summary>
        public int TweetId { get; set; }

        /// <summary>
        /// Gets or sets the profile reference of who liked the <see cref="Tweet"/>.
        /// </summary>
        public ProfileReferenceEntity Author { get; set; }

        /// <summary>
        /// Gets or sets the accociated tweet.
        /// </summary>
        public virtual TweetEntity Tweet { get; set; }
    }
}
