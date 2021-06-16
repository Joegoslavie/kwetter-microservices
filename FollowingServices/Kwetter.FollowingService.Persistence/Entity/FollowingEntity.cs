namespace Kwetter.FollowingService.Persistence.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class FollowingEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int FollowingId { get; set; }

        /// <summary>
        /// Gets or sets the profile reference table.
        /// </summary>
        public virtual ProfileReferenceEntity UserProfile { get; set; }

        /// <summary>
        /// Gets or sets the profile reference table.
        /// </summary>
        public virtual ProfileReferenceEntity FollowingProfile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime FollowingSince { get; set; } = DateTime.Now;
    }
}
