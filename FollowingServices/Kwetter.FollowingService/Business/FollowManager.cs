using Kwetter.FollowingService.Business.Enum;
using Kwetter.FollowingService.Persistence.Context;
using Kwetter.FollowingService.Persistence.Entity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> ToggleFollower(int currentId, string username)
        {
            var profileRef = this.context.ProfileReferences.SingleOrDefault(x => x.Username == username);
            var myProfileRef = this.context.ProfileReferences.SingleOrDefault(x => x.UserId == currentId);

            var entity = this.context.Followings.FirstOrDefault(x => x.UserId == currentId && x.FollowingId == profileRef.UserId);

            if (entity == null)
            {
                entity = new FollowingEntity
                {
                    UserId = currentId,
                    FollowingId = profileRef.UserId,
                    FollowingProfile = profileRef,
                    UserProfile = myProfileRef,
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

        public async Task<List<FollowingEntity>> GetFollowing(string username, int page, int amount)
        {
            var profileRef = this.context.ProfileReferences.SingleOrDefault(x => x.Username == username);
            if (profileRef == null)
            {
                throw new KeyNotFoundException(nameof(username));
            }

            return this.context.Followings
                .Where(f => f.UserProfile.UserId == profileRef.UserId)
                .OrderByDescending(x => x.FollowingSince)
                .Include(x => x.FollowingProfile)
                .Skip(page * amount)
                .Take(amount)
                .ToList();
        }

        public async Task<List<FollowingEntity>> GetFollowers(string username, int page, int amount)
        {
            var profileRef = this.context.ProfileReferences.SingleOrDefault(x => x.Username == username);
            if (profileRef == null)
            {
                throw new KeyNotFoundException(nameof(username));
            }

            return this.context.Followings
                .Where(f => f.FollowingProfile.UserId == profileRef.UserId)
                .OrderByDescending(x => x.FollowingSince)
                .Include(x => x.UserProfile)
                .Skip(page * amount)
                .Take(amount)
                .ToList();
        }

        public async Task<bool> ToggleBlocked(int currentId, string username)
        {
            var profileRef = this.context.ProfileReferences.SingleOrDefault(x => x.Username == username);

            var entity = this.context.Blocked.FirstOrDefault(x => x.UserId == currentId && x.BlockedId == profileRef.UserId);

            if (entity == null)
            {
                entity = new BlockEntity
                {
                    UserId = currentId,
                    BlockedId = profileRef.UserId,
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
