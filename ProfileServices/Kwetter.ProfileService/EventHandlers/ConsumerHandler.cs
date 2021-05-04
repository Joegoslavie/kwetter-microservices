namespace Kwetter.ProfileService.EventHandlers
{
    using Confluent.Kafka;
    using Kwetter.ProfileService.Persistence.Context;
    using Microsoft.Extensions.Hosting;
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
        /// <param name="stoppingToken">CancellationToken.</param>
        /// <returns>Task yo.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = this.consumer.Consume(stoppingToken);
                    if (consumeResult != null)
                    {
                        var value = consumeResult.Message.Value;
                        // profiel maken.
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
    }
}
