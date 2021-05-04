namespace Kwetter.ProfileService.EventHandlers
{
    using Confluent.Kafka;
    using Kwetter.ProfileService.Persistence.Context;
    using Kwetter.ProfileService.Persistence.Entity;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ConsumerHandler : BackgroundService
    {
        /// <summary>
        /// Topic name.
        /// </summary>
        private readonly string topic = "kwetter_pf";

        /// <summary>
        /// Consumes the shit out of Kwetter.
        /// </summary>
        private readonly IConsumer<Ignore, string> consumer;

        private readonly ProfileContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerHandler"/> class.
        /// </summary>
        /// <param name="consumer">Consumer.</param>
        /// <param name="context">Context.</param>
        public ConsumerHandler(IConsumer<Ignore, string> consumer, ProfileContext context)
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
                        this.context.Profiles.Add(new ProfileEntity
                        {
                            UserId = args.Id,
                            Username = args.Username,
                            DisplayName = args.Username,
                        });

                        await this.context.SaveChangesAsync(token).ConfigureAwait(false);

                        this.consumer.Commit();
                        Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occured: {e.Error.Reason}");
                }
            }
        }

        class ProfileEventArgs
        {
            public int Id { get; set; }

            public string Username { get; set; }
        }
    }
}
