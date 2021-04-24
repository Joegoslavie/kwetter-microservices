namespace Kwetter.TweetService.Services
{
    using Kwetter.TweetService.Persistence.Context;
    using Microservice.TweetGRPCService;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Tweet service class.
    /// </summary>
    public class TweetService : TweetGRPCService.TweetGRPCServiceBase
    {
        /// <summary>
        /// Default logging instance.
        /// </summary>
        private readonly ILogger<TweetService> logger;

        /// <summary>
        /// Tweet context instance.
        /// </summary>
        private readonly TweetContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TweetService"/> class.
        /// </summary>
        /// <param name="logger">Injected logger.</param>
        /// <param name="context">Injected context.</param>
        public TweetService(ILogger<TweetService> logger, TweetContext context)
        {
            this.logger = logger;
            this.context = context;
        }
    }
}
