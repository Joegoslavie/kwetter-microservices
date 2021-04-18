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

        public override async Task<OperationResponse> ToggleFollow(FollowRequest request, ServerCallContext context)
        {
            try
            {
                var result = await this.manager.ToggleFollower(request.UserId, request.FollowingId).ConfigureAwait(false);

                switch (result)
                {
                    case Operation.Created:
                        return new OperationResponse { Status = true, Message = string.Empty, };
                    case Operation.Removed:
                        return new OperationResponse { Status = true, Message = string.Empty, };
                    default:
                        return new OperationResponse { Status = false, Message = string.Empty, };
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception occurred!", ex);
                throw;
            }
        }

        public override async Task<OperationResponse> ToggleBlock(BlockRequest request, ServerCallContext context)
        {
            try
            {
                var result = await this.manager.ToggleBlocked(request.UserId, request.BlockId).ConfigureAwait(false);

                switch (result)
                {
                    case Operation.Created:
                        return new OperationResponse { Status = true, Message = string.Empty, };
                    case Operation.Removed:
                        return new OperationResponse { Status = true, Message = string.Empty, };
                    default:
                        return new OperationResponse { Status = false, Message = string.Empty, };
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception occurred!", ex);
                throw;
            }
        }

    }
}
