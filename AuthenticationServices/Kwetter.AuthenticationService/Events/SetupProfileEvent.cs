namespace Kwetter.AuthenticationService.Events
{
    using Confluent.Kafka;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class SetupProfileEvent : ISetupProfileEvent, IDisposable
    {
        private readonly IProducer<string, string> producer;
        private readonly string topic;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupProfileEvent"/> class.
        /// </summary>
        /// <param name="producer">Producer.</param>
        /// <param name="topic">Topic string.</param>
        public SetupProfileEvent(IProducer<string, string> producer, string topic)
        {
            this.producer = producer;
            this.topic = topic;
        }

        public async void Invoke(int userId, string username)
        {
            try
            {
                var msg = JsonConvert.SerializeObject(new { Id = userId, Username = username });
                var result = await this.producer.ProduceAsync(
                        this.topic, new Message<string, string> { Value = msg }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
