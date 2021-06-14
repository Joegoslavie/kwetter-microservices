namespace Kwetter.TweetService.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Kwetter.Messaging.Interfaces.Tweet;
    using Kwetter.TweetService.Persistence.Context;
    using Kwetter.TweetService.Persistence.Entity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Tweet manager for retrieving and creating tweets.
    /// </summary>
    public class TweetManager
    {
        /// <summary>
        /// Regex used to determine mentions and hashtags.
        /// </summary>
        private const string EventRegex = @"(?:^|\s+)(?:(?<mention>@)|(?<hash>#))(?<item>\w+)(?=\s+)";

        /// <summary>
        /// Tweet context.
        /// </summary>
        private readonly TweetContext context;

        /// <summary>
        /// Logger instance.
        /// </summary>
        private readonly ILogger<TweetManager> logger;

        /// <summary>
        /// Event when a mention is present inside the to be placed tweet.
        /// </summary>
        private readonly ITweetEvent tweetMentionEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="TweetManager"/> class.
        /// </summary>
        /// <param name="logger">Injected logger.</param>
        /// <param name="mentionEvent">Mention event.</param>
        /// <param name="context">Injected context.</param>
        public TweetManager(ILogger<TweetManager> logger, ITweetEvent mentionEvent, TweetContext context)
        {
            this.context = context;
            this.logger = logger;
            this.tweetMentionEvent = mentionEvent;
        }

        /// <summary>
        /// Gets the tweets with the associated account.
        /// </summary>
        /// <param name="username">username.</param>
        /// <param name="page">Current page.</param>
        /// <param name="amount">Amount to retrieve.</param>
        /// <returns>Tweets.</returns>
        public List<TweetEntity> GetTweetsByUser(string username, int page, int amount)
        {
            return this.context.Tweets
                .Include(x => x.Author)
                .Where(x => x.Author.Username == username)
                .Include(x => x.LikedBy)
                .Include(x => x.Hashtags)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(page * amount)
                .Take(amount)
                .ToList();
        }

        /// <summary>
        /// Gets the tweets with the associated account.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <param name="page">Current page.</param>
        /// <param name="amount">Amount to retrieve.</param>
        /// <returns>Tweets.</returns>
        public List<TweetEntity> GetTweetsByUser(int userId, int page, int amount)
        {
            return this.context.Tweets
                .Include(x => x.Author)
                .Where(x => x.Author.UserId == userId)
                .Include(x => x.LikedBy)
                .Include(x => x.Hashtags)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(page * amount)
                .Take(amount)
                .ToList();
        }

        /// <summary>
        /// Creates a new <see cref="TweetEntity"/> including possible mentions and hashtags.
        /// </summary>
        /// <param name="userId">Invoker user id.</param>
        /// <param name="content">The content of the tweet.</param>
        /// <returns><see cref="TweetEntity"/></returns>
        public async Task<TweetEntity> CreateNewTweet(int userId, string content)
        {
            var profileRef = this.context.ProfileReferences.FirstOrDefault(x => x.UserId == userId);
            var tweet = new TweetEntity
            {
                Author = profileRef,
                Content = content,
                CreatedAt = DateTime.Now,
            };

            this.context.Tweets.Add(tweet);
            await this.context.SaveChangesAsync().ConfigureAwait(false);

            this.ScanForMentions(tweet);
            return tweet;
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
            var entity = this.context.Likes.Include(x => x.Author).FirstOrDefault(x => x.Author.UserId == userId && x.TweetId == tweetId);

            if (entity == null)
            {
                var profile = this.context.ProfileReferences.FirstOrDefault(x => x.UserId == userId);
                var tweet = this.context.Tweets.FirstOrDefault(x => x.Id == tweetId);

                if (profile != null && tweet != null)
                {
                    entity = new LikeEntity
                    {
                        Author = profile,
                        Tweet = tweet,
                        TweetId = tweetId,
                    };

                    this.context.Likes.Add(entity);
                }
            }
            else
            {
                this.context.Likes.Remove(entity);
                entity = null;
            }

            await this.context.SaveChangesAsync().ConfigureAwait(false);
            return entity != null;
        }

        /// <summary>
        /// Gets a collection of tweets that tagged the specified username.
        /// </summary>
        /// <param name="username">.</param>
        /// <param name="page">.</param>
        /// <param name="amount">.</param>
        /// <returns>.</returns>
        public async Task<List<TweetEntity>> GetMentionedTweets(string username, int page, int amount)
        {
            return this.context.Mentions
                .Include(x => x.DirectedTo)
                .Where(x => x.DirectedTo.Username == username)
                .Include(x => x.Tweet)
                .Select(x => x.Tweet)
                .Skip(page * amount)
                .Take(amount)
                .ToList();
        }

        /// <summary>
        /// Constructs timeline basd on user ids.
        /// </summary>
        /// <param name="userIds">User id collection.</param>
        /// <param name="page">Current page.</param>
        /// <param name="amount">Amount to retrieve.</param>
        /// <returns>Tweets</returns>
        public List<TweetEntity> TimelineOfUsers(IEnumerable<int> userIds, int page, int amount)
        {
            return this.context.Tweets
                .Include(x => x.Author)
                .Where(x => userIds.Contains(x.Author.UserId))
                .Include(x => x.LikedBy)
                .Include(x => x.Hashtags)
                .OrderBy(x => Guid.NewGuid())
                .Skip(page * amount)
                .Take(amount)
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public List<TweetEntity> RandomTimeline(int page, int amount)
        {
            return this.context.Tweets
                .Include(x => x.Author)
                .Include(x => x.LikedBy)
                .Include(x => x.Hashtags)
                .OrderBy(x => Guid.NewGuid())
                .Skip(page * amount)
                .Take(amount)
                .ToList();
        }

        /// <summary>
        /// Scans the tweet for possible mentions and hashtags.
        /// </summary>
        /// <param name="tweet">Tweet.</param>
        private void ScanForMentions(TweetEntity tweet)
        {
            var tagsMentions = Regex.Matches($"{tweet.Content} ", EventRegex);
            var matched = tagsMentions.Where(x => x.Success).ToList();

            var usernames =
                    matched
                        .Select(x => x.Value.Trim())
                        .Distinct()
                        .Where(x => x.StartsWith("@"))
                        .Select(x => x[1..])
                        .ToList();

            usernames.ForEach(username =>
            {
                var mentionedUser = this.context.ProfileReferences.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
                if (mentionedUser != null)
                {
                    this.tweetMentionEvent.Invoke(tweet.Id, tweet.Author.UserId, mentionedUser.UserId);
                }
            });
        }
    }
}
