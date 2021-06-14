using Grpc.Core;
using Kwetter.FollowingService.Business;
using Kwetter.FollowingService.Business.Enum;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.FollowingService.Services
{
    public class FollowingService : FollowGRPCService.FollowGRPCServiceBase
    {
        private readonly ILogger<FollowingService> logger;

        private readonly FollowManager manager;

        public FollowingService(ILogger<FollowingService> logger, FollowManager manager)
        {
            this.logger = logger;
            this.manager = manager;
        }

        public override async Task<OperationResponse> GetFollowIds(FollowerRequest request, ServerCallContext context)
        {
            int userId = request.UserId;
            var followingIds = this.manager.GetFollowingIds(userId);
            var followerIds = this.manager.GetFollowerIds(userId);

            var response = new OperationResponse
            {
                Status = true,
                Message = "Success",
            };

            response.Followers.AddRange(followerIds.Select(x => x.UserId));
            response.Following.AddRange(followingIds.Select(x => x.FollowingId));
            return response;
        }

        public override async Task<OperationResponse> ToggleFollow(FollowRequest request, ServerCallContext context)
        {
            try
            {
                return new OperationResponse
                {
                    Status = await this.manager.ToggleFollower(request.UserId, request.FollowingId).ConfigureAwait(false),
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }

        public override async Task<OperationResponse> ToggleBlock(BlockRequest request, ServerCallContext context)
        {
            try
            {
                return new OperationResponse
                {
                    Status = await this.manager.ToggleBlocked(request.UserId, request.BlockId).ConfigureAwait(false),
                };

            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }

    }
}
