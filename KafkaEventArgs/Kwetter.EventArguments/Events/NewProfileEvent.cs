namespace Kwetter.Messaging.Events
{
    using Confluent.Kafka;
    using Kwetter.Messaging.Arguments;
    using Kwetter.Messaging.Interfaces;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class NewProfileEvent : IProfileEvent, IDisposable
    {
        private readonly IProducer<string, string> producer;
        private readonly string topic;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewProfileEvent"/> class.
        /// </summary>
        /// <param name="producer">Producer.</param>
        /// <param name="topicName">Topic string.</param>
        public NewProfileEvent(IProducer<string, string> producer, string topicName)
        {
            this.producer = producer;
            this.topic = topicName;
        }

        /// <inheritdoc/>
        public void Invoke(int userId, string username, string displayName)
        {
            try
            {
                var content = new ProfileEventArgs
                {
                    UserId = userId,
                    Username = username,
                    DisplayName = displayName,
                };

                var jsonContents = JsonConvert.SerializeObject(content);
                this.producer.ProduceAsync(this.topic, new Message<string, string> { Value = jsonContents }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {

        }
    }
}
