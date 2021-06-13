namespace Kwetter.TweetService.Business
{
    using Kwetter.TweetService.Persistence.Context;
    using Kwetter.TweetService.Persistence.Entity;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Tweet manager for retrieving and creating tweets.
    /// </summary>
    public class TweetManager
    {
        /// <summary>
        /// Tweet context.
        /// </summary>
        private readonly TweetContext context;

        /// <summary>
        /// Logger instance.
        /// </summary>
        private readonly ILogger<TweetManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TweetManager"/> class.
        /// </summary>
        /// <param name="logger">Injected logger.</param>
        /// <param name="context">Injected context.</param>
        public TweetManager(ILogger<TweetManager> logger, TweetContext context)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the tweets with the associated account.
        /// </summary>
        /// <param name="username">username.</param>
        /// <returns>Tweets.</returns>
        public async Task<List<TweetEntity>> GetTweetsByUser(string username)
        {
            throw new Exception();
        }

        /// <summary>
        /// Gets the tweets with the associated account.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <returns>Tweets.</returns>
        public async Task<List<TweetEntity>> GetTweetsByUser(int userId)
        {
            throw new Exception();
        }

        /// <summary>
        /// Creates a new <see cref="TweetEntity"/> including possible mentions and hashtags.
        /// </summary>
        /// <param name="userId">Invoker user id.</param>
        /// <param name="content">The content of the tweet.</param>
        /// <returns><see cref="TweetEntity"/></returns>
        public async Task<TweetEntity> PlaceTweet(int userId, string content)
        {
            throw new Exception();
        }

        /// <summary>
        /// Toggles a <see cref="LikeEntity"/>, in case the tweet is already liked,
        /// the existing entity will be deleted.
        /// </summary>
        /// <param name="userId">Invoker user id.</param>
        /// <param name="tweetId">Id of the tweet.</param>
        /// <returns>True </returns>
        public async Task<bool> ToggleTweetLike(int userId, int tweetId)
        {
            throw new Exception();
        }

        /// <summary>
        /// Constructs timeline basd on user ids.
        /// </summary>
        /// <param name="userIds">User id collection.</param>
        /// <param name="page">Current page.</param>
        /// <param name="amount">Amount to retrieve.</param>
        /// <returns>Tweets</returns>
        public async Task<List<TweetEntity>> MakeTimelineOfUserId(IEnumerable<int> userIds, int page, int amount)
        {
            throw new Exception();
        }

    }
}
