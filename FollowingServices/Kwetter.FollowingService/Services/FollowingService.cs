using Grpc.Core;
using Kwetter.FollowingService.Business;
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

        public override Task<OperationResponse> ToggleFollow(FollowRequest request, ServerCallContext context)
        {
            return base.ToggleFollow(request, context);
        }

        public override Task<OperationResponse> ToggleBlock(BlockRequest request, ServerCallContext context)
        {
            return base.ToggleBlock(request, context);
        }

    }
}
