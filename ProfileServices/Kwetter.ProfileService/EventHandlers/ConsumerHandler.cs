namespace Kwetter.ProfileService.EventHandlers
{
    using Confluent.Kafka;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ConsumerHandler : BackgroundService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string topic = "kwetter_pf";
        
        /// <summary>
        /// 
        /// </summary>
        private readonly IConsumer<Ignore, string> consumer;

        public ConsumerHandler(IConsumer<Ignore, string> consumer)
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            throw new NotImplementedException();
        }
    }
}
