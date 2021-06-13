namespace Kwetter.TweetService.EventHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Kwetter.Messaging;
    using Kwetter.Messaging.Arguments;
    using Kwetter.TweetService.Persistence.Context;
    using Kwetter.TweetService.Persistence.Entity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;

    /// <summary>
    /// The Kafka handler for tweets.
    /// </summary>
    public class KafkTweetEventHandler : BackgroundService
    {
        /// <summary>
        /// Consumes the shit out of Kwetter.
        /// </summary>
        private readonly IConsumer<Ignore, string> consumer;

        /// <summary>
        /// Tweet context.
        /// </summary>
        private readonly TweetContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaEventHandler"/> class.
        /// </summary>
        /// <param name="consumer">Consumer.</param>
        /// <param name="context">Context.</param>
        public KafkTweetEventHandler(IConsumer<Ignore, string> consumer, TweetContext context)
        {
            this.consumer = consumer;
            this.context = context;
        }

        /// <summary>
        /// Execute.
        /// </summary>
        /// <param name="token">CancellationToken.</param>
        /// <returns>Task yo.</returns>
        protected override async Task ExecuteAsync(CancellationToken token)
        {
            await Task.Yield();
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = this.consumer.Consume(token);
                    if (consumeResult != null)
                    {
                        await this.ProcessTopic(consumeResult.Topic, consumeResult.Message.Value, token);
                        Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occured in topic {e.ConsumerRecord.Topic}: {e.Error.Reason}");
                }
            }
        }

        /// <summary>
        /// Process message pulled from Kafka broker.
        /// </summary>
        /// <param name="topicName">Name of the topic</param>
        /// <param name="messageContents">Message body.</param>
        /// <param name="token">Token.</param>
        /// <returns>Task.</returns>
        private async Task ProcessTopic (string topicName, string messageContents, CancellationToken token)
        {
            if (string.IsNullOrEmpty(topicName) || string.IsNullOrEmpty(messageContents))
            {
                return;
            }

            switch (topicName)
            {
                case EventSettings.NewTweetProfileEventTopic:
                case EventSettings.TweetProfileUpdateEventTopic:
                    var profileArgs = JsonConvert.DeserializeObject<ProfileEventArgs>(messageContents);
                    await this.CreateOrUpdateProfileRef(profileArgs).ConfigureAwait(false);
                    break;

                case EventSettings.NewTweetMentionEventTopic:
                    var mentionArgs = JsonConvert.DeserializeObject<MentionEventArgs>(messageContents);
                    await this.CreateTweetMention(mentionArgs).ConfigureAwait(false);
                    break;

            }

            this.consumer.Commit();
        }

        /// <summary>
        /// Creates a new mention record in the database only if there is no existing mention found.
        /// <param name="mentionArgs">Args.</param>
        /// <returns></returns>
        private async Task CreateTweetMention(MentionEventArgs mentionArgs)
        {
             if (this.context.Mentions.Include(x => x.DirectedTo).Where
                    (x => x.DirectedTo.UserId == mentionArgs.MentionUserId && x.TweetId == mentionArgs.TweetId).Any())
            {
                // the user is already mentioned in this tweet.
                return;
            }

             var tweet = this.context.Tweets.Include(x => x.Author).FirstOrDefault(x => x.Author.UserId == mentionArgs.TweetId);
             var mentionEntity = new MentionEntity
             {
                DirectedTo = this.context.ProfileReferences.FirstOrDefault(x => x.UserId == mentionArgs.MentionUserId),
                Tweet = tweet,
                TweetId = tweet.Id,
             };

             this.context.Mentions.Add(mentionEntity);
             await this.context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Creates or updates the <see cref="ProfileReferenceEntity"/> of the user associated with the arguments.
        /// </summary>
        /// <param name="profileArgs">Event data.</param>
        /// <returns>Task.</returns>
        private async Task CreateOrUpdateProfileRef(ProfileEventArgs profileArgs)
        {
            var entity = this.context.ProfileReferences.FirstOrDefault(p => p.UserId == profileArgs.UserId);

            if (entity == null)
            {
                entity = new ProfileReferenceEntity()
                {
                    UserId = profileArgs.UserId,
                    Username = profileArgs.Username,
                    AvatarUrl = "default.jpg",
                };

                this.context.ProfileReferences.Add(entity);
            }

            entity.DisplayName = profileArgs.DisplayName;
            await this.context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
