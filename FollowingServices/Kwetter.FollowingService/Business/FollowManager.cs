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

        public async Task<Operation> ToggleFollower(int currentId, int followId)
        {
            var entity = this.context.Followings.FirstOrDefault(x => x.UserId == currentId && x.FollowingId == followId);
            Operation result;

            if (entity == null)
            {
                this.context.Followings.Add(new FollowingEntity
                {
                    UserId = currentId,
                    FollowingId = followId,
                });
                result = Operation.Created;
            }
            else
            {
                this.context.Followings.Remove(entity);
                result = Operation.Removed;
            }

            await this.context.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<Operation> ToggleBlocked(int currentId, int blockId)
        {
            var entity = this.context.Blocked.FirstOrDefault(x => x.UserId == currentId && x.BlockedId == blockId);
            Operation result;

            if (entity == null)
            {
                this.context.Blocked.Add(new BlockEntity
                {
                    UserId = currentId,
                    BlockedId = blockId,
                });
                result = Operation.Created;
            }
            else
            {
                this.context.Blocked.Remove(entity);
                result = Operation.Removed;
            }

            await this.context.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }
    }
}
