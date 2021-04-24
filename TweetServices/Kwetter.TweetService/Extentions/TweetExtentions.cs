namespace Kwetter.TweetService.Extentions
{
    using System;
    using Kwetter.TweetService.Persistence.Entity;
    using Microservice.TweetGRPCService;

    /// <summary>
    /// Class with extention methods.
    /// </summary>
    public static class TweetExtentions
    {
        /// <summary>
        /// Converts a <see cref="TweetEntity"/> into a <see cref="TweetResponseData"/> object.
        /// </summary>
        /// <param name="tweet">Tweet to convert.</param>
        /// <returns>Converted entity.</returns>
        public static TweetResponseData Convert(this TweetEntity tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentNullException(nameof(tweet));
            }

            return new TweetResponseData
            {
                UserId = tweet.UserId,
                Username = tweet.Username,
                DisplayName = tweet.DisplayName,
                AvatarUrl = tweet.AvatarUrl,
                Content = tweet.Content,
                CreatedAt = tweet.CreatedAt.Ticks,
            };
        }
    }
}
