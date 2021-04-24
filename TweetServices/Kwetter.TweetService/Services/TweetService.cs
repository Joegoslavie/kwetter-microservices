namespace Kwetter.TweetService.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Kwetter.TweetService.Extentions;
    using Kwetter.TweetService.Persistence.Context;
    using Microservice.TweetGRPCService;
    using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Retrieves all tweets of the passed user id.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="TweetResponse"/>.</returns>
        public override async Task<TweetResponse> GetTweetsByUserId(TweetRequest request, ServerCallContext context)
        {
            return await Task.Run(() =>
            {
                var tweets = this.context.Tweets.Where(x => x.UserId == request.UserId).Include(x => x.LikedBy).Include(x => x.Hashtags).ToList();
                var response = new TweetResponse { Status = tweets.Any() };

                response.Tweets.AddRange(tweets.Select(t => t.Convert()));
                return response;
            });
        }

        /// <summary>
        /// Retrieves all tweets of the passed username.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="TweetResponse"/>.</returns>
        public override async Task<TweetResponse> GetTweetsByUsername(TweetRequest request, ServerCallContext context)
        {
            return await Task.Run(() =>
            {
                var tweets = this.context.Tweets.Where(x => x.Username == request.Username).Include(x => x.LikedBy).Include(x => x.Hashtags).ToList();
                var response = new TweetResponse { Status = tweets.Any() };

                response.Tweets.AddRange(tweets.Select(t => t.Convert()));
                return response;
            });
        }

        /// <summary>
        /// Retrieves all tweets of the passed user ids. Can be used to construct the timeline of a user.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="TweetResponse"/>.</returns>
        public override async Task<TweetResponse> GetTweetsByUserIds(TweetRequest request, ServerCallContext context)
        {
            return await Task.Run(() =>
            {
                var tweets = this.context.Tweets.Where(x => request.UserIds.Contains(x.UserId))
                    .Include(x => x.LikedBy)
                    .Include(x => x.Hashtags)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList();

                var response = new TweetResponse { Status = tweets.Any() };

                response.Tweets.AddRange(tweets.Select(t => t.Convert()));
                return response;
            });
        }
    }
}
