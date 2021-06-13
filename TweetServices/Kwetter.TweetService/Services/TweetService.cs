namespace Kwetter.TweetService.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Kwetter.TweetService.Business;
    using Kwetter.TweetService.Extentions;
    using Kwetter.TweetService.Persistence.Context;
    using Kwetter.TweetService.Persistence.Entity;
    using Microservice.TweetGRPCService;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Tweet service class.
    /// </summary>
    public class TweetService : TweetGRPCService.TweetGRPCServiceBase
    {

        // TODO: manage status, use manager, events

        /// <summary>
        /// Default logging instance.
        /// </summary>
        private readonly ILogger<TweetService> logger;

        /// <summary>
        /// Tweet manager instance.
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

        /// <summary>
        /// Retrieves all tweets of the passed user id.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="TweetResponse"/>.</returns>
        public override async Task<TweetResponse> GetTweetsByUserId(TweetRequest request, ServerCallContext context)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var tweets = this.manager.GetTweetsByUser(request.UserId, request.Page, request.Amount);
                    var response = new TweetResponse();
                    response.Tweets.AddRange(
                        tweets.Select(t => t.Convert()));

                    return response;
                });
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all tweets of the passed username.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="TweetResponse"/>.</returns>
        public override async Task<TweetResponse> GetTweetsByUsername(TweetRequest request, ServerCallContext context)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var tweets = this.manager.GetTweetsByUser(request.Username, request.Page, request.Amount);
                    var response = new TweetResponse();
                    response.Tweets.AddRange(
                        tweets.Select(t => t.Convert()));

                    return response;
                });
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves all tweets of the passed user ids. Can be used to construct the timeline of a user.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="TweetResponse"/>.</returns>
        public override async Task<TweetResponse> GetTweetsByUserIds(TweetRequest request, ServerCallContext context)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var tweets = this.manager.TimelineOfUsers(request.UserIds, request.Page, request.Amount);
                    var response = new TweetResponse();
                    response.Tweets.AddRange(
                        tweets.Select(t => t.Convert()));

                    return response;
                });
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }

        /// <summary>
        /// Creates a new tweet.
        /// </summary>
        /// <param name="request">req.</param>
        /// <param name="context">context.</param>
        /// <returns>The new tweet.</returns>
        public override async Task<TweetResponse> PlaceTweet(PlaceTweetRequest request, ServerCallContext context)
        {
            try
            {
                return await Task.Run(async () =>
                {
                    var tweet = await this.manager.CreateNewTweet(request.UserId, request.Content).ConfigureAwait(false);

                    var response = new TweetResponse();
                    response.Tweets.Add(tweet.Convert());
                    return response;
                });
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }

        /// <summary>
        /// Toggles a like on the tweet associated with the passed id. In case the user has not yet liked the tweet, the
        /// tweet will be liked. If the user already liked this tweet, it will be undone.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="TweetResponse"/>.</returns>
        public override async Task<TweetResponse> ToggleLike(TweetOperationRequest request, ServerCallContext context)
        {
            try
            {
                return await Task.Run(async () =>
                {
                    return new TweetResponse
                    {
                        Status = await this.manager.ToggleTweetLike(request.UserId, request.TweetId).ConfigureAwait(false),
                    };
                });
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }
    }
}
