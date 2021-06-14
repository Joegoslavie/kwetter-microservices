using Kwetter.FollowingService.Business.Enum;
using Kwetter.FollowingService.Persistence.Context;
using Kwetter.FollowingService.Persistence.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.FollowingService.Business
{
    public class FollowManager
    {
        private readonly FollowingContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowManager"/> class.
        /// </summary>
        /// <param name="context"></param>
        public FollowManager(FollowingContext context)
        {
            this.context = context;
        }

        public List<FollowingEntity> GetFollowerIds(int userId)
        {
            return this.context.Followings.Where(x => x.FollowingId == userId).ToList();
        }

        public List<FollowingEntity> GetFollowingIds(int userId)
        {
            return this.context.Followings.Where(x => x.UserId == userId).ToList();
        }

        public async Task<bool> ToggleFollower(int currentId, int followId)
        {
            var entity = this.context.Followings.FirstOrDefault(x => x.UserId == currentId && x.FollowingId == followId);
            if (entity == null)
            {
                entity = new FollowingEntity
                {
                    UserId = currentId,
                    FollowingId = followId,
                };

                this.context.Followings.Add(entity);
            }
            else
            {
                this.context.Followings.Remove(entity);
                entity = null;
            }

            await this.context.SaveChangesAsync().ConfigureAwait(false);
            return entity != null;
        }

        public async Task<bool> ToggleBlocked(int currentId, int blockId)
        {
            var entity = this.context.Blocked.FirstOrDefault(x => x.UserId == currentId && x.BlockedId == blockId);

            if (entity == null)
            {
                entity = new BlockEntity
                {
                    UserId = currentId,
                    BlockedId = blockId,
                };
                this.context.Blocked.Add(entity);
            }
            else
            {
                this.context.Blocked.Remove(entity);
                entity = null;
            }

            await this.context.SaveChangesAsync().ConfigureAwait(false);
            return entity != null;
        }
    }
}
