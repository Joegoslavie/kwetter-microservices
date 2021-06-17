namespace Kwetter.FollowingService.Services
{
    using Grpc.Core;
    using Kwetter.FollowingService.Business;
    using Kwetter.FollowingService.Business.Enum;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FollowingService : FollowGRPCService.FollowGRPCServiceBase
    {
        private readonly ILogger<FollowingService> logger;

        private readonly FollowManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowingService"/> class.
        /// </summary>
        /// <param name="logger">L.</param>
        /// <param name="manager">M.</param>
        public FollowingService(ILogger<FollowingService> logger, FollowManager manager)
        {
            this.logger = logger;
            this.manager = manager;
        }

        /// <summary>
        /// Gets following ids of the username.
        /// </summary>
        /// <param name="request">request.</param>
        /// <param name="context">context.</param>
        /// <returns>OperationResponse.</returns>
        public override async Task<OperationResponse> GetFollowingByUsername(FollowInfoRequest request, ServerCallContext context)
        {
            try
            {
                var result = new OperationResponse();
                var profiles = await this.manager.GetFollowing(request.Username, request.Page, request.Amount).ConfigureAwait(false);
                result.Profiles.AddRange(profiles.Select(p => new ProfileFollowResponse
                {
                    UserId = p.UserProfile.UserId,
                    Username = p.UserProfile.Username,
                    DisplayName = p.UserProfile.DisplayName,
                    AvatarUrl = p.UserProfile.AvatarUrl,
                    Since = p.FollowingSince.Ticks,
                }));

                return result;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }

        /// <summary>
        /// Gets follower ids of the username.
        /// </summary>
        /// <param name="request">request.</param>
        /// <param name="context">context.</param>
        /// <returns>OperationResponse.</returns>
        public override async Task<OperationResponse> GetFollowersByUsername(FollowInfoRequest request, ServerCallContext context)
        {
            try
            {
                var result = new OperationResponse();
                var profiles = await this.manager.GetFollowers(request.Username, request.Page, request.Amount).ConfigureAwait(false);
                result.Profiles.AddRange(profiles.Select(p => new ProfileFollowResponse
                {
                    UserId = p.UserProfile.UserId,
                    Username = p.UserProfile.Username,
                    DisplayName = p.UserProfile.DisplayName,
                    AvatarUrl = p.UserProfile.AvatarUrl,
                    Since = p.FollowingSince.Ticks,
                }));

                return result;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }

        /// <summary>
        /// Toggles a follow.
        /// </summary>
        /// <param name="request">request.</param>
        /// <param name="context">context.</param>
        /// <returns>OperationResponse.</returns>
        public override async Task<OperationResponse> ToggleFollow(ToggleFollowRequest request, ServerCallContext context)
        {
            try
            {
                return new OperationResponse { Status = await this.manager.ToggleFollower(request.UserId, request.Username).ConfigureAwait(false), };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }

        /// <summary>
        /// Toggles a block.
        /// </summary>
        /// <param name="request">request.</param>
        /// <param name="context">context.</param>
        /// <returns>OperationResponse.</returns>
        public override async Task<OperationResponse> ToggleBlock(ToggleBlockRequest request, ServerCallContext context)
        {
            try
            {
                return new OperationResponse { Status = await this.manager.ToggleBlocked(request.UserId, request.Username).ConfigureAwait(false), };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }

    }
}
