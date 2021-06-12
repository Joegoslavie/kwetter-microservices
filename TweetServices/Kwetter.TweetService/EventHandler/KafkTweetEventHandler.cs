namespace Kwetter.TweetService.EventHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Kwetter.Messaging.Arguments;
    using Kwetter.TweetService.Persistence.Context;
    using Kwetter.TweetService.Persistence.Entity;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;

    public class KafkTweetEventHandler : BackgroundService
    {
        /// <summary>
        /// Consumes the shit out of Kwetter.
        /// </summary>
        private readonly IConsumer<Ignore, string> consumer;

        /// <summary>
        /// 
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
                        var args = JsonConvert.DeserializeObject<ProfileEventArgs>(consumeResult.Message.Value);
                        await this.CreateOrUpdate(args, token).ConfigureAwait(false);
                        Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occured: {e.Error.Reason}");
                }
            }
        }

        private async Task CreateOrUpdate(ProfileEventArgs profileArgs, CancellationToken token)
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
            }

            entity.DisplayName = profileArgs.DisplayName;
            await this.context.SaveChangesAsync(token).ConfigureAwait(false);

            this.consumer.Commit();
        }
    }
}
