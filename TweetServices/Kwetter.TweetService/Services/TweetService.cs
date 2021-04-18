namespace Kwetter.TweetService.Services
{
    using Grpc.Core;
    using Kwetter.TweetService.Business;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class TweetService : TweetGRPCService.TweetGRPCServiceBase
    {
        /// <summary>
        /// Default logging instance.
        /// </summary>
        private readonly ILogger<TweetService> logger;

        /// <summary>
        /// Manager class for tweets.
        /// </summary>
        private readonly TweetManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TweetService"/> class.
        /// </summary>
        /// <param name="logger">Injected logger.</param>
        /// <param name="manager">Injected manager.</param>
        public TweetService(ILogger<TweetService> logger, TweetManager manager)
        {
            this.logger = logger;
            this.manager = manager;
        }

        public override Task<TweetResponse> GetByUser(RetrieveTweetRequest request, ServerCallContext context)
        {
            return base.GetByUser(request, context);
        }

        public override Task<TweetResponse> GetByHashtag(RetrieveTweetRequest request, ServerCallContext context)
        {
            return base.GetByHashtag(request, context);
        }

        public override Task<TweetResponse> PlaceTweet(TweetOperationRequest request, ServerCallContext context)
        {
            return base.PlaceTweet(request, context);
        }

        public override Task<TweetResponse> ToggleLike(TweetOperationRequest request, ServerCallContext context)
        {
            return base.ToggleLike(request, context);
        }

    }
}
