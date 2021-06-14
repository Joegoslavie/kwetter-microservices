namespace Kwetter.FollowingService.EventHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Kwetter.FollowingService.Persistence.Context;
    using Kwetter.FollowingService.Persistence.Entity;
    using Kwetter.Messaging.Arguments;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;

    class KafkaEventHandler : BackgroundService
    {
        /// <summary>
        /// Consumes the shit out of Kwetter.
        /// </summary>
        private readonly IConsumer<Ignore, string> consumer;

        private readonly FollowingContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaEventHandler"/> class.
        /// </summary>
        /// <param name="consumer">Consumer.</param>
        /// <param name="context">Context.</param>
        public KafkaEventHandler(IConsumer<Ignore, string> consumer, FollowingContext context)
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
                        await this.InitializeProfile(args, token).ConfigureAwait(false);
                        Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occured: {e.Error.Reason}");
                }
            }
        }

        private async Task InitializeProfile(ProfileEventArgs profileArgs, CancellationToken token)
        {
            if (this.context.ProfileReferences.Any(x => x.UserId == profileArgs.UserId))
            {
                return;
            }

            this.context.ProfileReferences.Add(new ProfileReferenceEntity
            {
                UserId = profileArgs.UserId,
                Username = profileArgs.Username,
            });

            await this.context.SaveChangesAsync(token).ConfigureAwait(false);
            this.consumer.Commit();
        }
    }
}
