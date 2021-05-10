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
    public class BlockEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BlockedId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime BlockedAt { get; set; } = DateTime.Now;
    }
}
